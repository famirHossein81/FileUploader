const slider_container = document.getElementById("slider_container");
const slides = document.getElementsByClassName("slide");

let currentSlideIndex = 0;
let intervalId;

const startAutoSlider = () => {
  intervalId = setInterval(sliderRight, 5000);
};

const resetTimer = () => {
  clearInterval(intervalId);
  startAutoSlider();
};

const updateSlideWidth = () => {
  resetTimer();

  let slideWidth = parseFloat(
    window.getComputedStyle(slides[currentSlideIndex]).getPropertyValue("width")
  );
  const newLeftPosition = `-${currentSlideIndex * slideWidth}px`;
  slider_container.style.left = newLeftPosition;
  if (currentSlideIndex === 0) {
    document
      .querySelector("#slider_btn > span:nth-of-type(1)")
      .classList.add("active");
  } else {
    document
      .querySelector("#slider_btn > span:nth-of-type(1)")
      .classList.remove("active");
  }
  if (currentSlideIndex === 1) {
    document
      .querySelector("#slider_btn > span:nth-of-type(2)")
      .classList.add("active");
  } else {
    document
      .querySelector("#slider_btn > span:nth-of-type(2)")
      .classList.remove("active");
  }
  if (currentSlideIndex === 2) {
    document
      .querySelector("#slider_btn > span:nth-of-type(3)")
      .classList.add("active");
  } else {
    document
      .querySelector("#slider_btn > span:nth-of-type(3)")
      .classList.remove("active");
  }
  if (currentSlideIndex === 3) {
    document
      .querySelector("#slider_btn > span:nth-of-type(4)")
      .classList.add("active");
  } else {
    document
      .querySelector("#slider_btn > span:nth-of-type(4)")
      .classList.remove("active");
  }
};

const sliderRight = () => {
  currentSlideIndex++;
  if (currentSlideIndex >= slides.length) {
    currentSlideIndex = 0;
  }

  updateSlideWidth();
};

const sliderLeft = () => {
  currentSlideIndex--;
  if (currentSlideIndex < 0) {
    currentSlideIndex = slides.length - 1;
  }
  updateSlideWidth();
};

window.addEventListener("resize", () => {
  slider_container.style.left = 0;
  currentSlideIndex = 0;
  updateSlideWidth();
});

startAutoSlider();
