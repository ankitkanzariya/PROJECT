from tkinter import *

# option select karva mate nu select function
def select(event):
    b=event.widget
    value=b['text']

    for i in range(15):
        if value==correct_answers[i]:
            questionArea.delete(1.0,END)
            questionArea.insert(END,questions[i+1])

            optionButton1.config(text=first_option[i+1])
            optionButton2.config(text=second_option[i+1])
            optionButton3.config(text=third_option[i+1])
            optionButton4.config(text=fourth_option[i+1])
            amountLable.config(image=amountImages[i])

        else:
            root1=Toplevel()
            root1.config(bg='black')
            root1.geometry('500x400+140+30')
            root1.title('You won 0 pounds')
            imgLabel=Label(root1,image=centerImage,bd=0)
            imgLabel.pack(padY=30)

            root1.mainloop()
            break



correct_answers = [
    "Ankit", "B.C.A", "22", "Python", "India",
    "5", "Blue", "Car", "Sachin Tendulkar", "Elephant",
    "Earth", "Pizza", "Mango", "Chess", "Mount Everest"
]

questions = [
    "What's your name?",
    "Which bachelor's degree have you completed?",
    "What's your age?",
    "Which programming language is known for its simplicity?",
    "Which country is known as the 'Land of Spices'?",
    "How many continents are there?",
    "What color is the sky on a clear day?",
    "What is the most common type of vehicle?",
    "Who is known as the 'God of Cricket'?",
    "Which animal is the largest land animal?",
    "Which planet is known as the 'Blue Planet'?",
    "Which food is commonly associated with Italy?",
    "Which fruit is known as the 'King of Fruits'?",
    "Which game is known as the 'Game of Kings'?",
    "Which is the highest mountain in the world?"
]

first_option = [
    "Ankit", "M.C.A", "20", "Python", "India",
    "5", "Blue", "Car", "Sachin Tendulkar", "Elephant",
    "Earth", "Pizza", "Mango", "Chess", "Mount Everest"
]

second_option = [
    "Mangal", "B.C.A", "21", "Java", "China",
    "6", "Red", "Bus", "Virat Kohli", "Lion",
    "Mars", "Burger", "Apple", "Ludo", "K2"
]

third_option = [
    "Keshav", "B.A", "22", "C++", "USA",
    "7", "Green", "Bike", "MS Dhoni", "Tiger",
    "Venus", "Pasta", "Banana", "Poker", "Kangchenjunga"
]

fourth_option = [
    "Sambhu", "Btech", "23", "JavaScript", "Brazil",
    "8", "Yellow", "Truck", "Brian Lara", "Giraffe",
    "Jupiter", "Sushi", "Grapes", "Snakes and Ladders", "Lhotse"
]


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

rightframe = Frame(root, bg='black', pady=25, padx=50)
rightframe.grid(row=0, column=1)

image50 = PhotoImage(file='50-50.png')
lifeline50button = Button(topFrame, image=image50, bg='black', bd=0, activebackground='black', width=180, height=80)
lifeline50button.grid(row=0, column=0, padx=0, pady=0)

audiencepole = PhotoImage(file='audiencePole.png')
audiencepoleButton = Button(topFrame, image=audiencepole, bg='black', bd=0, activebackground='black', width=180,
                            height=80)
audiencepoleButton.grid(row=0, column=1, padx=0, pady=0)

photoImage = PhotoImage(file='phoneAFriend.png')
phoneLifelineButton = Button(topFrame, image=photoImage, bg='black', bd=0, activebackground='black', width=180,
                             height=80)
phoneLifelineButton.grid(row=0, column=2, padx=0, pady=0)

centerImage = PhotoImage(file='center.png')
logoLabel = Label(centerFrame, image=centerImage, bg='black', width=300, height=200)
logoLabel.grid(row=0, column=0, padx=0, pady=0)

amountimage = PhotoImage(file='picture0.png')
amountimage1 = PhotoImage(file='picture1.png')
amountimage2 = PhotoImage(file='picture2.png')
amountimage3 = PhotoImage(file='picture3.png')
amountimage4 = PhotoImage(file='picture4.png')
amountimage5 = PhotoImage(file='picture5.png')
amountimage6 = PhotoImage(file='picture5.png')
amountimage7 = PhotoImage(file='picture7.png')
amountimage8 = PhotoImage(file='picture8.png')
amountimage9 = PhotoImage(file='picture9.png')
amountimage10 = PhotoImage(file='picture10.png')
amountimage11 = PhotoImage(file='picture11.png')
amountimage12 = PhotoImage(file='picture12.png')
amountimage13 = PhotoImage(file='picture13.png')
amountimage14 = PhotoImage(file='picture14.png')
amountimage15 = PhotoImage(file='picture15.png')

amountImages=[amountimage1,amountimage2,amountimage3,amountimage4,amountimage5,amountimage6,amountimage7,amountimage8,amountimage9,amountimage10,amountimage11,amountimage12,amountimage13,amountimage14,amountimage15]


amountLable = Label(rightframe, image=amountimage, bg='black')
amountLable.grid(row=0, column=0)

layoutImage = PhotoImage(file='lay.png')
layoutLable = Label(bottomFrame, image=layoutImage, bg='black')
layoutLable.grid(row=0, column=0)

# for question area
questionArea = Text(bottomFrame, font=('arial', 18, 'bold'), width=34, height=2, wrap='word', bg='black', fg='white',
                    bd=0)
questionArea.place(x=70, y=10)

# for question
questionArea.insert(END, questions[0])

# for options
labelA = Label(bottomFrame, text='A:', bg='black', fg='white', font=('arial', 16, 'bold'))
labelA.place(x=60, y=110)
optionButton1 = Button(bottomFrame, text=first_option[0], bg='black', cursor='hand2', bd=0, fg='white',
                       activebackground='black', activeforeground='white', font=('arial', 18, 'bold'))
optionButton1.place(x=100, y=100)

labelB = Label(bottomFrame, text='B:', bg='black', fg='white', font=('arial', 16, 'bold'))
labelB.place(x=330, y=110)
optionButton2 = Button(bottomFrame, text=second_option[0], bg='black', cursor='hand2', bd=0, fg='white',
                       activebackground='black', activeforeground='white', font=('arial', 18, 'bold'))
optionButton2.place(x=370, y=100)

labelC = Label(bottomFrame, text='C:', bg='black', fg='white', font=('arial', 16, 'bold'))
labelC.place(x=60, y=190)
optionButton3 = Button(bottomFrame, text=third_option[0], bg='black', cursor='hand2', bd=0, fg='white',
                       activebackground='black', activeforeground='white', font=('arial', 18, 'bold'))
optionButton3.place(x=100, y=180)

labelD = Label(bottomFrame, text='D:', bg='black', fg='white', font=('arial', 16, 'bold'))
labelD.place(x=330, y=190)
optionButton4 = Button(bottomFrame, text=fourth_option[0], bg='black', cursor='hand2', bd=0, fg='white',
                       activebackground='black', activeforeground='white', font=('arial', 18, 'bold'))
optionButton4.place(x=370, y=180)

optionButton1.bind('<Button-1>', select)
optionButton2.bind('<Button-1>', select)
optionButton3.bind('<Button-1>', select)
optionButton4.bind('<Button-1>', select)

root.mainloop()