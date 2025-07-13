///

function includepopuphtml() {
  let html =
    '<div id="popup"><span id="close" onclick = "close_popup()" ><img src="image/close.png" alt="" id="x"></span><img src="image/left-arrow.png" alt="" id="left"><img src="image/right-arrow.png" alt="" id="right"><img src="image/london.jpg" alt="" id="mainpop"></div>';

  let popdiv = document.createElement("div");
  popdiv.innerHTML = html;
  document.body.insertBefore(popdiv, document.body.firstChild);
}

includepopuphtml();

// function to init plugin
let img;
let current;

function imgpopupinit(target) {
  //select all images with class target
  img = document.getElementsByClassName(target);

  //add event listner on all selected img

  for (var i = 0; i < img.length; i++) {
    // add pointer
    img[i].style.cursor = "pointer";

    // add event listner
    img[i].addEventListener("click", function () {
      document.getElementById("mainpop").src = this.src;
      document.getElementById("popup").style.display = "block";
      checkarrow();
    });
  }

  //next button
  document.getElementById("right").addEventListener("click", function () {
    nexting();
  });

  //next img
  function nexting() {
    // alert("next...");
    getcurrentimg();
    current++;
    document.getElementById("mainpop").src = img[current].src;
    checkarrow();
  }

  //prev button
  document.getElementById("left").addEventListener("click", function () {
    preving();
  });
}

//prev img
function preving() {
  // alert("prev...");
  getcurrentimg();
  current--;
  document.getElementById("mainpop").src = img[current].src;
  checkarrow();
}

//close button
function close_popup() {
  document.getElementById("mainpop").src = "";
  document.getElementById("popup").style.display = "none";
}

//current img
function getcurrentimg() {
  i = 0;
  i < img.length;
  i++;
  {
    if (img[i].src == document.getElementById("mainpop").src) {
      current = i;
    }
  }
}

//check arrow
function checkarrow() {
  getcurrentimg();

  if (current == "0") {
    document.getElementById("left").style.display = "none";
    document.getElementById("right").style.display = "block";
  } else if (current == img.length - 1) {
    document.getElementById("right").style.display = "none";
    document.getElementById("left").style.display = "block";
  } else {
    document.getElementById("right").style.display = "block";
    document.getElementById("left").style.display = "block";
  }
}
