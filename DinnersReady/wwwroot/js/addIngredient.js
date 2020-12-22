// get form food categories when new ingredient singular name is entered
firstFoodCategoryInput = true;
$(document).ready(function () {
    if (firstFoodCategoryInput == true) {
        let ingredientNameFeedbackParaId = document.getElementById('ingredientNameFeedbackParaId');
        let newIngredientSingularId = "newIngredientSingularId";
        let newIngredientFoodCategoryId = "newIngredientFoodCategoryId";
        $("#" + newIngredientSingularId).on("input", function () {
            $("#" + newIngredientFoodCategoryId).empty();
            let options = {};
            options.url = "/Add/GetFoodCategories";
            options.type = "GET";
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + newIngredientFoodCategoryId).empty();
                for (let s = 0; s < data.length; s++) {
                    var opt = document.createElement('option');
                    opt.value = data[s].trim();
                    opt.innerHTML = data[s].trim();
                    $("#" + newIngredientFoodCategoryId).append(opt);
                }
            };
            $.ajax(options);
        });
    }
    window.firstFoodCategoryInput = false;
});


function processIngredient()
{
    // clear fields
    prepareIngredFeedbackDiv();
    let ingredientNameFeedbackDivId = document.getElementById('ingredientNameFeedbackDivId');
    ingredientNameFeedbackDivId.classList.remove('show');
    ingredientNameFeedbackDivId.classList.add('hide');
    ingredientNameFeedbackDivId.style.height = 0;
    ingredientNameFeedbackDivId.style.fontSize = 0;
    let ingredientNameFeedbackParaId = document.getElementById('ingredientNameFeedbackParaId');
    ingredientNameFeedbackParaId.classList.remove('show');
    ingredientNameFeedbackParaId.classList.add('hide');  
    ingredientNameFeedbackParaId.innerHTML = "";
    ingredientNameFeedbackParaId.style.marginBottom = 0;

    if(checkAddIngredientsFields() == 0)
    {
        // disable the Add New ingredient buttom
        document.getElementById('addIngredientFormId').disabled = true;

        // prep variables to be used
        let ingredientNameSingular = "newIngredientSingularId";
        let ingredientNamePlural = "newIngredientPluralId";
        let ingredientFoodCategory = "newIngredientFoodCategoryId";
        let ingredientDryOrWetMeasure = "dryOrWetMeasureId";
        
        // check whether an Ingredient with these values already exists
        let options = {};
        options.url = "/Add/ProcessNewIngredient";
        options.type = "GET";
        options.dataType = "json";
        options.async = true;
        options.data =
        {   "ingredientNameSingular":       $("#" + ingredientNameSingular).val(),
            "ingredientNamePlural":         $("#" + ingredientNamePlural).val(),
            "ingredientFoodCategory":       $("#" + ingredientFoodCategory).val(),
            "ingredientDryOrWetMeasure":    $("#" + ingredientDryOrWetMeasure).val()  };

        options.success = function (data) {

            var dataFromJson = JSON.stringify(data).trim();
            var forCompare = "\"IngredientNotAdded\"";
            var result = dataFromJson.localeCompare(forCompare);

            if(result != 0) {
                let updateMessage = "New Ingredient '" + dataFromJson + "' added!";
                alertify.alert('Success', updateMessage, function() {
                    document.getElementById('addIngredientFormId').disabled = false;
                    window.firstFoodCategoryInput = true;
                    closeAddIngredientForm();
                });
            }else{
                let newIngredientSingularId = document.getElementById('newIngredientSingularId').value;
                let updateMessage2 = "Ingredient '" + newIngredientSingularId + "' already exists!";
                alertify.alert('Warning', updateMessage2, function() {
                });           

                document.getElementById('addIngredientFormId').disabled = false;
                window.firstFoodCategoryInput = true;
            }
        };
        $.ajax(options);
    }else{
        let ingredientNameFeedbackDivId = document.getElementById('ingredientNameFeedbackDivId');
        ingredientNameFeedbackDivId.classList.remove('hide');
        ingredientNameFeedbackDivId.classList.add('show');
        ingredientNameFeedbackDivId.style.height = '40px';
        ingredientNameFeedbackDivId.style.marginTop = '10px';
        ingredientNameFeedbackDivId.style.color = '#ff0000';
        ingredientNameFeedbackDivId.style.fontSize = '25px';
        let ingredientNameFeedbackParaId = document.getElementById('ingredientNameFeedbackParaId');
        ingredientNameFeedbackParaId.innerHTML = "Please complete all fields";
        ingredientNameFeedbackParaId.classList.remove('hide');
        ingredientNameFeedbackParaId.classList.add('show');
    }
}


function prepareIngredFeedbackDiv(){
    var feedbackDiv = document.getElementById('ingredientNameFeedbackDivId');
    var newFeedbackPara = document.createElement('paragraph');
    newFeedbackPara.id = 'ingredientNameFeedbackParaId';
    newFeedbackPara.classList.add('hide');
    feedbackDiv.appendChild(newFeedbackPara);
    ingredFeedBackAvailable = true;
}


ingredFeedBackAvailable = true;
$(document).ready(function () {
    $('.ingredientElement').on("input", function () {
        if(ingredFeedBackAvailable == true){
            var feedbackPara = document.getElementById('ingredientNameFeedbackParaId');
            feedbackPara.parentNode.removeChild(feedbackPara);
            var feedbackDiv = document.getElementById('ingredientNameFeedbackDivId');
            feedbackDiv.classList.remove('show');
            feedbackDiv.classList.add('add');
            ingredFeedBackAvailable = false;
        }
    }); 
});


function checkAddIngredientsFields() {
    var fieldIds = ['newIngredientSingularId', 'newIngredientPluralId', 
                    'newIngredientFoodCategoryId', 'dryOrWetMeasureId'];
    var populatedFields = 0;
    fieldIds.forEach(function (item, index) {
        populatedFields += checkEachAddIngredientField(item);
    });
    let elem = document.getElementById('newIngredientButton');
    elem.scrollIntoView();
    return populatedFields;
}


function checkEachAddIngredientField(fieldId){
    var notPopulated = 0;    
    var eleVal = document.getElementById(fieldId).value;
    if(eleVal.length == 0) {
        notPopulated = 1;
            var elemt = document.getElementById(fieldId);          
            elemt.classList.remove('heartbeat');
            elemt.classList.add('rubberBand');
    }
    return notPopulated;
}


function closeAddIngredientForm() {
    let cancelButton = window.document.getElementById("closeAddIngredientFormButton");
    cancelButton.parentNode.removeChild(cancelButton);    
    let addIngredDiv = window.document.getElementById("formDiv");
    addIngredDiv.parentNode.removeChild(addIngredDiv);

    // move the ingredient fields into view
    if(ingredCounter == 5){
        let elem = document.getElementById('ingredientsId' + (ingredCounter - 1));  
        elem.scrollIntoView();  
    }else{
        let elem = document.getElementById('newIngredientButton');
        elem.scrollIntoView();  
    }  
}






