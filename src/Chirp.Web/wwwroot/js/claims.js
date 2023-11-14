window.onload = function () {
    fetch(GithubURL)
        .then((response) => {
            if (!response.ok) {
                throw new Error("Network response was not ok");
            }
            return response.json();
        })
        .then((data) => {
            console.log("GitHub API Response:", data); // Log the entire data object

            document.querySelector(".api-id").textContent = data.id || "";
            document.querySelector(".api-login").textContent = data.login || "";
            document.querySelector(".api-name").textContent = data.name || "";
            document.querySelector(".api-avatar_url").src = data.avatar_url || "";
        })
        .catch((error) => {
            console.error("Error fetching data:", error);
        });
};
