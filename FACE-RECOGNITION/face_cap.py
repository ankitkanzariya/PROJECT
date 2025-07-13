import cv2
import sqlite3
import os

# Initialize face detector
face_cap = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_frontalface_default.xml")
video_cap = cv2.VideoCapture(0)

# Create database connection
conn = sqlite3.connect("face_database.db")
cursor = conn.cursor()

# Create table if it doesn't exist
cursor.execute("CREATE TABLE IF NOT EXISTS faces (id INTEGER PRIMARY KEY, name TEXT, image_path TEXT)")
conn.commit()

# Get user name
person_name = input("Enter your name: ")
folder = "faces"
os.makedirs(folder, exist_ok=True)  # Create folder if not exists

count = 0

while True:
    ret, frame = video_cap.read()
    if not ret:
        continue

    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)
    faces = face_cap.detectMultiScale(gray, 1.2, 6, minSize=(40, 40))

    for (x, y, w, h) in faces:
        count += 1
        img_path = f"{folder}/{person_name}_{count}.jpg"
        cv2.imwrite(img_path, frame[y:y + h, x:x + w])  # Save cropped face

        # Store in database
        cursor.execute("INSERT INTO faces (name, image_path) VALUES (?, ?)", (person_name, img_path))
        conn.commit()

        # Draw a rectangle
        cv2.rectangle(frame, (x, y), (x + w, y + h), (0, 255, 0), 2)

    cv2.imshow("Face Capture", frame)

    if count >= 5:  # Capture 5 images
        break

    if cv2.waitKey(1) & 0xFF == ord("q"):
        break

video_cap.release()
cv2.destroyAllWindows()
conn.close()
