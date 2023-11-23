// Collapse and expand sections of the About Me page
function toggleSection(sectionId) {
    var section = document.getElementById(sectionId);
    var icon = document.getElementById(sectionId + "Icon");

    if (section.style.display === "none") {
        section.style.display = "block";
        icon.textContent = "▲";
    } else {
        section.style.display = "none";
        icon.textContent = "▼";
    }
}
