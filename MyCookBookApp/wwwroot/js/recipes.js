document.addEventListener("DOMContentLoaded", function () {
    const BASE_URL = `${window.location.origin}/Recipe`;  // Dynamically set base URL
    let selectedCategories = [];

    // Define a mapping of category numbers to their text values
    const categoryMapping = {
        0: "Breakfast",
        1: "Lunch",
        2: "Dinner",
        3: "Dessert",
        4: "Snack",
        5: "Vegan",
        6: "Vegetarian",
        7: "GlutenFree",
        8: "Keto",
        9: "LowCarb",
        10: "HighProtein"
    };

    fetchRecipes();

    // Handle category selection
    document.querySelectorAll("#categoryList .dropdown-item").forEach(item => {
        item.addEventListener("click", function (event) {
            event.preventDefault();
            let categoryValue = parseInt(this.getAttribute("data-value"), 10);
            let categoryText = this.innerText;

            if (selectedCategories.includes(categoryValue)) {
                // If already selected, remove it
                selectedCategories = selectedCategories.filter(c => c !== categoryValue);
            } else {
                // Otherwise, add it
                selectedCategories.push(categoryValue);
            }

            // Update button text to display selected categories
            document.getElementById("categoryDropdown").innerText =
                selectedCategories.length > 0 ? selectedCategories.map(id => document.querySelector(`[data-value="${id}"]`).innerText).join(", ") : "Select Categories";

            // Update hidden input with selected categories
            document.getElementById("categories").value = JSON.stringify(selectedCategories);

            console.log("Selected Categories (Numeric):", selectedCategories);
        });
    });
    
    // Open the Add Recipe Modal
    document.getElementById("addRecipeBtn")?.addEventListener("click", function () {
        document.getElementById("addRecipeForm").reset();
        $('#addRecipeModal').modal('show');
    });

    // Modify Save Recipe function to include selected numeric categories
    document.getElementById("saveRecipe").addEventListener("click", function (event) {
        event.preventDefault();

        let recipe = {
            recipeId: "NewRecipe_Only",
            name: document.getElementById("recipeName").value.trim(),
            tagLine: document.getElementById("tagLine").value.trim(),
            summary: document.getElementById("summary").value.trim(),
            ingredients: document.getElementById("ingredients").value.split(",").map(i => i.trim()),
            instructions: document.getElementById("instructions").value.split("\n").map(i => i.trim()),

            // Get selected categories as numbers
            categories: JSON.parse(document.getElementById("categories").value || "[]"),

            media: []
        };

        console.log("Final Recipe Object:", recipe);
        // Debugging - Displays formatted JSON
        console.log("Updated Recipe Object:", JSON.stringify(recipe, null, 2));

        // Send recipe data to API
        fetch(`${BASE_URL}/Add`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(recipe)
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                console.log("Recipe added successfully!");
                // ✅ Close the modal
                let modal = new bootstrap.Modal(document.getElementById('addRecipeModal'));
                modal.hide();

                // ✅ Refresh the page after a slight delay
                setTimeout(() => {
                    location.reload();
                }, 500); // 500ms delay to ensure UI updates smoothly
            } else {
                alert("Failed to add recipe: " + data.message);
            }
        })
        .catch(error => console.error("Error adding recipe:", error));
    });

    // Fetch all recipes
    function fetchRecipes() {
        console.log("Fetching all recipes...");

        fetch(`${BASE_URL}/GetAll`)
            .then(response => response.json())
            .then(recipes => {
                console.log("Recipes loaded:", recipes);
                displayRecipes(recipes);
            })
            .catch(error => console.error("Fetch error:", error));
    }

    // Search for recipes
    window.searchRecipes = function () {
        let keyword = document.getElementById("searchInput").value.trim();
        if (!keyword) {
            fetchRecipes();
            return;
        }

        let searchRequest = { keyword: keyword, categories: [] };

        fetch(`${BASE_URL}/Search`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(searchRequest)
        })
        .then(response => response.json())
        .then(recipes => {
            displayRecipes(recipes);
        })
        .catch(error => console.error("Search error:", error));
    };

    // Handle Enter key press for search
    window.handleEnter = function (event) {
        if (event.key === "Enter") {
            searchRecipes();
        }
    };

    // Show or hide the clear search button
    window.toggleClearButton = function () {
        let searchInput = document.getElementById("searchInput");
        let clearButton = document.getElementById("clearSearch");
        clearButton.style.display = searchInput.value ? "block" : "none";
    };

    // Clear search and reload all recipes
    window.clearSearch = function () {
        document.getElementById("searchInput").value = "";
        document.getElementById("clearSearch").style.display = "none";
        fetchRecipes();
    };

    // Display recipes dynamically
    function displayRecipes(recipes) {
        let cardContainer = document.getElementById("recipeCards");
        cardContainer.innerHTML = "";

        recipes.forEach(recipe => {
            let imageUrl = recipe.media && recipe.media.length > 0 ? recipe.media[0].url : "placeholder.jpg";
            // let categories = recipe.categories ? recipe.categories.join(" | ") : "N/A";
            // ✅ Convert category numbers to text labels
            let categoriesText = (recipe.categories || []).map(catId => categoryMapping[catId] || "Unknown").join(" | ");

            let card = `<div class="mb-3">
                <div class="card shadow-sm">
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h5 class="card-title mb-0">${recipe.name}</h5>
                                <p class="text-muted mb-2">${recipe.tagLine || ""}</p>
                            </div>
                        </div>
                        <div class="row mt-2">
                            <div class="col-md-6">
                                <h6>Chef's Note</h6>
                                <p class="card-text">${recipe.summary || "No summary available."}</p>
                                <h6>Ingredients</h6>
                                <p class="card-text">${recipe.ingredients ? recipe.ingredients.join(", ") : "N/A"}</p>
                            </div>
                            <div class="col-md-6">
                                <h6>Instructions</h6>
                                <p class="card-text">${recipe.instructions ? recipe.instructions.join("<br>") : "N/A"}</p>
                            </div>
                        </div>
                    </div>
                    <div class="card-footer text-muted text-left" style="font-size: 0.85rem;">
                        <b>Categories: </b>${categoriesText}
                    </div>
                </div>
            </div>`;

            cardContainer.innerHTML += card;
        });
    }
});
