// #region hex

document.addEventListener("DOMContentLoaded", function () {
  const hex = document.getElementById("hex");
  let hex_height, page_width;
  const hexes = document.getElementsByClassName("strength_hex");

  const updateWidth = () => {
    hex_height = parseFloat(
      window.getComputedStyle(hex).getPropertyValue("height")
    );
    hex_height = hex_height / 4;
    page_width = window.innerWidth;
    hexes[3].style.bottom = hex_height + "px";
    hexes[4].style.bottom = hex_height + "px";
    if (page_width <= 768) {
      hexes[2].style.bottom = hex_height + "px";
      hexes[3].style.bottom = 2 * hex_height + "px";
      hexes[4].style.bottom = 2 * hex_height + "px";
    }
    else {
      hexes[2].style.bottom = 0;
    }
    if (page_width <= 520) {
      hexes[1].style.bottom = hex_height + "px";
      hexes[2].style.bottom = 2 * hex_height + "px";
      hexes[3].style.bottom = 3 * hex_height + "px";
      hexes[4].style.bottom = 4 * hex_height + "px";
    } else {
      hexes[1].style.bottom = 0;
    }
  };

  const initialize = () => {
    updateWidth();
  };

  initialize();

  window.addEventListener("resize", () => {
    updateWidth();
  });
});

// #endregion hex
