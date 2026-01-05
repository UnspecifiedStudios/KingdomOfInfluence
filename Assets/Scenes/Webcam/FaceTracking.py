import cv2
import mediapipe as mp
import socket
import json
import sys
import traceback

def excepthook(type, value, tb):
    print("UNCAUGHT EXCEPTION:")
    traceback.print_exception(type, value, tb)

sys.excepthook = excepthook

# UDP setup
UDP_IP = "127.0.0.1"
UDP_PORT = 5005
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Initialize MediaPipe Face Mesh
mp_face_mesh = mp.solutions.face_mesh
face_mesh = mp_face_mesh.FaceMesh(
    static_image_mode=False,
    max_num_faces=1,
    refine_landmarks=True,  # Includes iris tracking
    min_detection_confidence=0.5,
    min_tracking_confidence=0.5
)

# Drawing utils
mp_drawing = mp.solutions.drawing_utils
drawing_spec = mp_drawing.DrawingSpec(thickness=1, circle_radius=1)

# Start webcam
cap = cv2.VideoCapture(0)

while cap.isOpened():
    success, frame = cap.read()
    if not success:
        break

    # Flip and convert image to RGB
    frame = cv2.flip(frame, 1)
    rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)

    # Process face mesh
    results = face_mesh.process(rgb)

    if results.multi_face_landmarks:
        for face_landmarks in results.multi_face_landmarks:

            def euclidean_distance(p1, p2):
                return ((p1.x - p2.x)**2 + (p1.y - p2.y)**2) ** 0.5

            # Mouth landmarks
            left_mouth = face_landmarks.landmark[61]
            right_mouth = face_landmarks.landmark[291]
            upper_lip = face_landmarks.landmark[14]
            lower_lip = face_landmarks.landmark[13]

            # Eye landmarks
            left_eye_top = face_landmarks.landmark[159]
            left_eye_bottom = face_landmarks.landmark[145]
            right_eye_top = face_landmarks.landmark[386]
            right_eye_bottom = face_landmarks.landmark[374]

            # Measurements
            mouth_width = euclidean_distance(left_mouth, right_mouth)
            mouth_open = euclidean_distance(upper_lip, lower_lip)

            leftSad = lower_lip.y - left_mouth.y
            rightSad = lower_lip.y - right_mouth.y
            cornerToBottom = (leftSad + rightSad) / 2

            left_eye_open = euclidean_distance(left_eye_top, left_eye_bottom)
            right_eye_open = euclidean_distance(right_eye_top, right_eye_bottom)

            face_width = euclidean_distance(
                face_landmarks.landmark[127],
                face_landmarks.landmark[356]
            )

            norm_mouth_width = mouth_width / face_width
            norm_mouth_open = mouth_open / face_width
            norm_eye_open = (left_eye_open + right_eye_open) / (2 * face_width)

            expression = ""

            if norm_mouth_width > 0.4 and norm_mouth_open < 0.05:
                expression = "(˶ᵔ ᵕ ᵔ˶)"
            elif norm_mouth_open > 0.08:
                expression = "( O _ O )"
            elif norm_eye_open < 0.02:
                expression = "(ᴗ˳ᴗ)ᶻ"
            elif cornerToBottom < -0.015:
                expression = "(╥.╥)"

            if expression:
                print(expression)
                try:
                    data = json.dumps({"expression": expression}).encode("utf-8")
                    print("SENDING UDP:", expression)
                    sock.sendto(data, (UDP_IP, UDP_PORT))
                except Exception as e:
                    print("UDP SEND ERROR:", e)
                except OSError:
                    pass

    # Show window
    cv2.imshow('MediaPipe Face Mesh', frame)
    if cv2.waitKey(1) & 0xFF == 27:
        break  # ESC to exit

cap.release()
cv2.destroyAllWindows()
