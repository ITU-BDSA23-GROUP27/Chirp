// pop up alert box

function confirmAction(action) {
    var confirmationMessage = "";

    if (action === 'reset') {
        confirmationMessage = "Are you sure you want to reset the database?";
    } else if (action === 'clear') {
        confirmationMessage = "Are you sure you want to clear the database?";
    }

    return confirm(confirmationMessage);
}
