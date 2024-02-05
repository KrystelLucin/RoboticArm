from cvzone.HandTrackingModule import HandDetector
import cv2
import socket
import math
import json
import numpy as np

def calculate_angle(pointA, pointB):
    # Calcular el vector entre los dos puntos
    vectorAB = [pointB[0] - pointA[0], pointB[1] - pointA[1]]
    # Calcular el ángulo con respecto al eje horizontal
    angle = math.atan2(vectorAB[1], vectorAB[0])
    # Convertir a grados y normalizar
    angle_degrees = math.degrees(angle) % 360
    return angle_degrees

def calculate_perpendicular_vector(p1, p2):
    # Vector directo entre p1 y p2
    vector = [p2[0] - p1[0], p2[1] - p1[1]]
    # Vector perpendicular
    perpendicular_vector = [-vector[1], vector[0]]
    return perpendicular_vector

def calculate_relative_angle(vector1, vector2):
    dot_product = vector1[0] * vector2[0] + vector1[1] * vector2[1]
    norm1 = (vector1[0]**2 + vector1[1]**2)**0.5
    norm2 = (vector2[0]**2 + vector2[1]**2)**0.5
    cos_angle = dot_product / (norm1 * norm2)
    angle = math.acos(cos_angle)
    return math.degrees(angle)

# Configuración de la cámara
width, height = 1280, 720
cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

# Detector de mano
detector = HandDetector(detectionCon=0.8, maxHands=1)

# Configuración del socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)

while True:
    # Obtener imagen de la cámara
    success, img = cap.read()
    if not success:
        continue

    # Detectar la mano y sus landmarks
    hands, img = detector.findHands(img)  # Con dibujo

    if hands:
        # Tomar la primera mano detectada
        hand = hands[0]
        lmList = hand["lmList"]  # Lista de 21 puntos de referencia

        # Asegurarse de que se detectaron los puntos necesarios
        if len(lmList) >= 9:
            # Calcula el vector de referencia perpendicular
            reference_vector = calculate_perpendicular_vector(lmList[2], lmList[5])

            # Calcula los ángulos relativos para el índice y el pulgar
            index_vector = [lmList[8][0] - lmList[6][0], lmList[8][1] - lmList[6][1]]
            thumb_vector = [lmList[4][0] - lmList[2][0], lmList[4][1] - lmList[2][1]]
            index_angle = calculate_relative_angle(reference_vector, index_vector)
            thumb_angle = calculate_relative_angle(reference_vector, thumb_vector)
            # Invertir el ángulo del pulgar antes de enviarlo
            thumb_angle = 360 - thumb_angle

            # Calcular los ángulos
            wrist_angle = calculate_angle(lmList[0], lmList[5])
            #index_angle = calculate_angle(lmList[6], lmList[8])
            #thumb_angle = calculate_angle(lmList[2], lmList[4])

            # Crear un diccionario con los ángulos
            angles = {
                "wrist": wrist_angle,
                "index": index_angle,
                "thumb": thumb_angle
            }

            # Convertir el diccionario a JSON
            json_data = json.dumps(angles)

            # Enviar los ángulos como JSON a través del socket
            sock.sendto(str.encode(json_data), serverAddressPort)

    # Mostrar imagen
    img = cv2.resize(img, (0, 0), None, 0.5, 0.5)
    cv2.imshow("Image", img)
    cv2.waitKey(1)
