from tkinter import *

questions = ["what's your name?",
             "Which beachlour degree you have complated?",
             "what's your age?"]

first_option = ["Ankit","B.C.A","22"]
second_option = ["mangal","M.C.A","22"]
third_option = ["keshav","B.A","22"]
fourth_option = ["sambhu","Btech","22"]

root = Tk()

root.geometry('1270x652+0+0')
root.title('Who Wants to Be a Billionaire')

root.config(bg='black')

leftframe = Frame(root, bg='black', padx=90)
leftframe.grid(row=0, column=0)

topFrame = Frame(leftframe, pady=15, bg='black')  # Ensure the frame itself has no background color issues
topFrame.grid()

centerFrame = Frame(leftframe, pady=15, bg='black')
centerFrame.grid(row=1, column=0)

bottomFrame = Frame(leftframe, bg='black')
bottomFrame.grid(row=2, column=0)

rightframe = Frame(root, bg='black',pady=25,padx=50)
rightframe.grid(row=0, column=1)

image50 = PhotoImage(file='50-50.png')
lifeline50button = Button(topFrame, image=image50, bg='black', bd=0, activebackground='black', width=180,height=80)
lifeline50button.grid(row=0, column=0, padx=0, pady=0)

audiencepole = PhotoImage(file='audiencePole.png')
audiencepoleButton = Button(topFrame, image=audiencepole, bg='black', bd=0, activebackground='black', width=180,height=80)
audiencepoleButton.grid(row=0, column=1, padx=0, pady=0)

photoImage = PhotoImage(file='phoneAFriend.png')
phoneLifelineButton = Button(topFrame, image=photoImage, bg='black', bd=0, activebackground='black', width=180,height=80)
phoneLifelineButton.grid(row=0, column=2, padx=0, pady=0)

centerImage = PhotoImage(file='center.png')
logoLabel = Label(centerFrame, image=centerImage, bg='black', width=300, height=200)
logoLabel.grid(row=0, column=0, padx=0, pady=0)

amountimage = PhotoImage(file='picture0.png')
amountLable=Label(rightframe,image=amountimage,bg='black')
amountLable.grid(row=0,column=0)

layoutImage = PhotoImage(file='lay.png')
layoutLable=Label(bottomFrame,image=layoutImage,bg='black')
layoutLable.grid(row=0,column=0)

# for question area
questionArea=Text(bottomFrame,font=('arial',18,'bold'),width=34,height=2,wrap='word',bg='black',fg='white',bd=0)
questionArea.place(x=70,y=10)

# for question
questionArea.insert(END,questions[0])

# for options
labelA=Label(bottomFrame,text='A:',bg='black',fg='white',font=('arial',16,'bold'))
labelA.place(x=60,y=110)
optionButton1=Button(bottomFrame,text=first_option[0],bg='black',cursor='hand2',bd=0,fg='white',activebackground='black',activeforeground='white',font=('arial',18,'bold'))
optionButton1.place(x=100,y=100)

labelB=Label(bottomFrame,text='B:',bg='black',fg='white',font=('arial',16,'bold'))
labelB.place(x=330,y=110)
optionButton2=Button(bottomFrame,text=second_option[0],bg='black',cursor='hand2',bd=0,fg='white',activebackground='black',activeforeground='white',font=('arial',18,'bold'))
optionButton2.place(x=370,y=100)

labelC=Label(bottomFrame,text='C:',bg='black',fg='white',font=('arial',16,'bold'))
labelC.place(x=60,y=190)
optionButton3=Button(bottomFrame,text=third_option[0],bg='black',cursor='hand2',bd=0,fg='white',activebackground='black',activeforeground='white',font=('arial',18,'bold'))
optionButton3.place(x=100,y=180)

labelD=Label(bottomFrame,text='D:',bg='black',fg='white',font=('arial',16,'bold'))
labelD.place(x=330,y=190)
optionButton4=Button(bottomFrame,text=fourth_option[0],bg='black',cursor='hand2',bd=0,fg='white',activebackground='black',activeforeground='white',font=('arial',18,'bold'))
optionButton4.place(x=370,y=180)



root.mainloop()
