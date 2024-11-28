import sys
from time import sleep
import pyautogui
try:
    pyautogui.keyDown(str(sys.argv[1]))
    sleep(float(sys.argv[2]))
    pyautogui.keyUp(str(sys.argv[1]))
except:
    x = sys.argv[1]
    for char in x:
        pyautogui.keyDown(str(char))
        sleep(float(sys.argv[2]))
        pyautogui.keyUp(str(char))