import sys
from time import sleep
import pyautogui
try:
    x = sys.argv[1]
    for char in x:
        pyautogui.keyDown(str(char))
        sleep(float(sys.argv[2]))
        pyautogui.keyUp(str(char))
except:
    sleep(float(sys.argv[2]))
    print("Hello World")