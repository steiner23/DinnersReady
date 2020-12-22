// Uses ajax calls to get either the Dry or Wet Units of Measure based upon the ingredient
// for drop-down select menu and then to select the the Original Unit of Measure
// For Instructions, loads the Temperature Units of Measure and selects
// the Original Unit of Measure

$(document).ready(function ()
{
    var ingredCount = document.getElementById('ingredientCountVId').value;
    for (let y = 1; y <= ingredCount; y++) {
        let ingredientsId = "ingredientsVId" + y.toString();
        let ingredientsUOMId = "ingredientsVUOMId" + y.toString();
        let ingredientsUOMIdHidden = "ingredientsVUOMIdOld" + y.toString();
        let ingredientsUOMIdHiddenValue = $("#" + ingredientsUOMIdHidden).val();

        $("#" + ingredientsUOMId).empty();
        let options = {};
        options.url = "/View/GetUnitsOfMeasureView";
        options.type = "GET";
        options.data = { "ingredient": $("#" + ingredientsId).val() };
        options.dataType = "json";
        options.async = false;
        options.success = function (data)
        {
            if (ingredientsUOMIdHiddenValue.trim() == "Item ( Items )") {
                var opt = document.createElement('option');
                opt.value = "Item ( Items )";
                opt.innerHTML = "Item ( Items )";
                opt.selected = true;
                $("#" + ingredientsUOMId).append(opt);
                document.getElementById(ingredientsUOMId).setAttribute('disabled', true);
            }
            else {
                for (let m = 0; m < data.length; m++) {
                    if (data[m].trim() !== "Item ( Items )")
                    {
                        var opt2 = document.createElement('option');
                        opt2.value = data[m].trim();
                        opt2.innerHTML = data[m].trim();
                        if (data[m].trim() === ingredientsUOMIdHiddenValue.trim()) {
                            opt2.selected = true;
                        } // end if
                        $("#" + ingredientsUOMId).append(opt2);
                    } // end if
                } // end for
            } // end else
        }; // end function
        $.ajax(options);
    } // end for

    var instructCount = document.getElementById('instructsCountVId').value;        
    var tempUOMs;

    let options = {};
    options.url = "/View/GetTemperatureUnitsOfMeasure";
    options.type = "GET";
    options.dataType = "json";
    options.async = false;
    options.success = function (data) {
        tempUOMs = data;
    };
    $.ajax(options);

    for (let z = 1; z <= instructCount; z++) {
        let instructionTempUoMId = "instructionVTempUoMId" + z.toString();
        let instructionTempUoMIdHidden = "instructionVTempUOMIdOld" + z.toString();
        let instructionTempUoMIdHiddenValue = $("#" + instructionTempUoMIdHidden).val();
        $("#" + instructionTempUoMId).empty();

        if (instructionTempUoMIdHiddenValue.trim() == "--none--") {
            var opt = document.createElement('option');
            opt.value = "--none--";
            opt.innerHTML = "--none--";
            opt.selected = true;
            $("#" + instructionTempUoMId).append(opt);                    
            document.getElementById(instructionTempUoMId).setAttribute('disabled', true);
        }
        else {
            for (let m = 0; m < tempUOMs.length; m++) {
                var opt3 = document.createElement('option');
                opt3.value = tempUOMs[m].trim();
                opt3.innerHTML = tempUOMs[m].trim();
                if (tempUOMs[m].trim() === instructionTempUoMIdHiddenValue.trim()) {
                    opt3.selected = true;
                } // end if
                $("#" + instructionTempUoMId).append(opt3);
            } // end for
        } // end else            
    } // end for       
});


// monitor Ingredient UoM fields for user selection changes a5nd re-calculate the amounts
ingredCounter = document.getElementById('ingredientCountVId').value;
$(document).ready(function ()
{
    for (let x = 1; x <= ingredCounter; x++) {
        let currentUoMStringValue = "ingredientsVUOMId" + x.toString();
        let ingredientsVFullAmount = "ingredientsVFullAmount" + x.toString();
        let currentAmountField = "ingredientsVAmountId" + x.toString();

        $("#" + currentUoMStringValue).on("change", function () {
            let options = {};
            options.url = "/View/GetNewUoMAmountValue";
            options.type = "GET";
            options.data = {
                "IngredFullAmount": $("#" + ingredientsVFullAmount).val(),  
                "NewUoMValue": $("#" + currentUoMStringValue).val()  
            };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + currentAmountField).empty();
                $("#" + currentAmountField).val(data);
            };
            $.ajax(options);
        });
    }
});


// monitor Instruction Temperature UoM fields for user selection changes and re-calculate the values
instructCounter = document.getElementById('instructsCountVId').value;
$(document).ready(function ()
{
    for (let x = 1; x <= instructCounter; x++) {
        let currentUoMStringValue = "instructionVTempUoMId" + x.toString();
        let temperatureVFullAmount = "instructionVFullAmount" + x.toString();
        let currentTemptField = "instructionVTempId" + x.toString();

        $("#" + currentUoMStringValue).on("change", function () {
            let options = {};
            options.url = "/View/GetNewUoMAmountValue";
            options.type = "GET";
            options.data = {
                "IngredFullAmount": $("#" + temperatureVFullAmount).val(),
                "NewUoMValue": $("#" + currentUoMStringValue).val()
            };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + currentTemptField).empty();
                $("#" + currentTemptField).val(data);
            };
            $.ajax(options);
        });
    }
});


// increment User Likes total for this recipe
function likeRecipe(RecipeIdent) {
    let options = {};
    options.url = "/View/AddUserLike";
    options.type = "POST";
    options.data = { "RecipeId": RecipeIdent };
    options.dataType = "json";
    options.async = true;
    options.success = function (data)
    {
        $('#likeRecipeTotal').html(data);
        $('#likeRecipeButton').prop("disabled", true);
        $('#likeRecipeTotal').css({ opacity: 0.65});
    };
    $.ajax(options);
}


function editRecipe() {
    $('#recipesLoading').addClass("backg overlay lds-dual-ring");    
    var ident = $('#recipeIdent').val();
    $.ajax({
        url: "/Edit/Recipe?ident=" + ident,
        type: "POST",
        async: true
    }).done(function (data) {
        $("#viewTarget").html(data);
        $('#recipesLoading').removeClass("backg overlay lds-dual-ring");        
    });
}


/*
// expand textareas to match content
window.onload = function() {
    textarea = document.querySelector(".autoresizing");
    textarea.addEventListener('input', autoResize, true);
    function autoResize() {
        this.style.height = 'auto';
        this.style.height = this.scrollHeight + 'px';
    }
};
*/