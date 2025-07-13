// replace with your emailjs user id
emailjs.init("eVsGSkLuoZyvKJMaJ");

const sendBtn = document.querySelector('.send-btn');
const result = document.querySelector('.result');

sendBtn.addEventListener('click',sendEmail);

function sendEmail(){
    //get the form data
    const to = document.getElementById("to").value;
    const subject = document.getElementById("subject").value;
    const message = document.getElementById("message").value;

//send emails using emailjs
// ------ 1st id serviceID 2nd id email templete ni setting ni templete id..
    emailjs.send("service_gfj7i08", "template_bw2cu6s",{
        to_email: to,
        subject: subject,
        message: message
    }).then(function(){
        result.innerHTML = "Email sent sucessfully";
        result.style.opacity=1;
    },function(error){
        result.innerHTML = "FAILED!!!!!!!!";
        result.style.opacity=1;
        console.log(error);
    })
}