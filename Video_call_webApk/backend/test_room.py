import requests
import json

BASE_URL = "http://127.0.0.1:8000"

def test_room_creation_and_joining():
    print("=== Testing Room Creation and Joining ===\n")
    
    # Step 1: Create a room
    print("1. Creating a room...")
    room_data = {
        "name": "Test Room for Demo",
        "password": "test123",
        "creator_id": "creator_user_001",
        "creator_username": "Alice"
    }
    
    try:
        response = requests.post(f"{BASE_URL}/rooms/", json=room_data)
        if response.status_code == 201:
            created_room = response.json()
            print(f"[SUCCESS] Room created successfully!")
            print(f"   Room ID: {created_room['id']}")
            print(f"   Room Name: {created_room['name']}")
            print(f"   Creator: {created_room['creator_id']}")
            room_id = created_room['id']
        else:
            print(f"[ERROR] Failed to create room: {response.status_code}")
            print(f"   Response: {response.text}")
            return
    except Exception as e:
        print(f"[ERROR] Error creating room: {e}")
        return
    
    print("\n" + "="*50 + "\n")
    
    # Step 2: Search for the room
    print("2. Searching for room...")
    try:
        response = requests.get(f"{BASE_URL}/rooms/{room_id}/search")
        if response.status_code == 200:
            room_info = response.json()
            print(f"[SUCCESS] Room found!")
            print(f"   Room ID: {room_info['id']}")
            print(f"   Room Name: {room_info['name']}")
        else:
            print(f"[ERROR] Room not found: {response.status_code}")
    except Exception as e:
        print(f"[ERROR] Error searching room: {e}")
    
    print("\n" + "="*50 + "\n")
    
    # Step 3: Request to join room
    print("3. Requesting to join room...")
    join_data = {
        "room_id": room_id,
        "password": "test123",
        "user_id": "joiner_user_002",
        "username": "Bob",
        "message": "Hi, I'd like to join your room!"
    }
    
    try:
        response = requests.post(f"{BASE_URL}/rooms/join", json=join_data)
        if response.status_code == 200:
            join_result = response.json()
            print(f"[SUCCESS] Join request sent successfully!")
            print(f"   Message: {join_result['message']}")
            print(f"   Request ID: {join_result['request_id']}")
        else:
            print(f"[ERROR] Failed to join room: {response.status_code}")
            print(f"   Response: {response.text}")
    except Exception as e:
        print(f"[ERROR] Error joining room: {e}")
    
    print("\n=== Test Complete ===")
    print(f"\n[INFO] Browser Testing:")
    print(f"   1. Open http://127.0.0.1:3001 (Tab 1)")
    print(f"   2. Create room with Room ID: {room_id}")
    print(f"   3. Open http://127.0.0.1:3001 (Tab 2)")
    print(f"   4. Join room with ID: {room_id}")
    print(f"   5. Password: test123")

if __name__ == "__main__":
    test_room_creation_and_joining()