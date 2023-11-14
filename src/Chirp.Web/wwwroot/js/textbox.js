// textarea - character counter
const textArea = document.getElementById("text");
const charCounter = document.getElementById("char-counter");

textArea.addEventListener("input", function () {
    charCounter.textContent = textArea.value.length;
});

// toggle button
const toggleButton = document.querySelector(".toggleButton");
const textboxContainer = document.querySelector(".textbox-container");

toggleButton.addEventListener("click", function () {
    textboxContainer.classList.toggle("collapsed");
    toggleButton.classList.toggle("active-color")
});