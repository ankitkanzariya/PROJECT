import qrcode as qr

img = qr.make("https://github.com/ankitkanzariya")
img.save("github.png")
