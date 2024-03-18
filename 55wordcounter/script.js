let textbox = document.getElementById("textbox");
textbox.addEventListener('input',function(){
    var text = this.value;
    let char = text.length;
    document.getElementById("char").innerHTML = char;

    //word count krva mate

    text = text.trim(); //trim function thi aagd pa6ad ni white space dur krva..

    let words = text.split(" ");
    let cleanlist = words.filter(function(elm){
        return elm != "";
    });
   // console.log("words");
    //console.log("cleanlist");

    //document.getElementById("word").innerHTML = words.length;

    document.getElementById("word").innerHTML = cleanlist.length;

});
