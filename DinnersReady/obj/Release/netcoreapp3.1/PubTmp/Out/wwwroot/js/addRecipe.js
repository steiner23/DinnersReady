/*
// display message to complete required fields and highlight these fields
function displayMessage() {
    console.log("Get to here?"); ///////////////////////////////////////////////////////////
    let outputDiv = document.getElementById('recipeFeedbackDivId');
    outputDiv.classList.remove('hide');
    outputDiv.classList.add('show');
    outputDiv.style.height = '40px';
    outputDiv.style.marginTop = '10px';
    outputDiv.style.color = '#262228';
    outputDiv.style.fontSize = '25px';
    let outputPara = document.getElementById('recipeFeedbackParaId');
    outputPara.classList.remove('hide');
    outputPara.classList.add('show');
    outputPara.style.visibility = "visible";
    outputPara.style.fontStyle = "italic";
    outputPara.innerHTML = "Please Complete All Required Fields:";
    let mainHeader = document.getElementById('mainHeader');
    mainHeader.scrollIntoView();
    mainHeader.style.marginTop = '145px';
    checkAddRecipeFields();
    console.log("Get to here also?"); ///////////////////////////////////////////////////////////    
}
*/


// fires up functions to monitor the 5 initial Ingredients fields and
// use ajax calls to get Ingredient name suggestions from the database
$(document).ready(function () {
    for (let x = 1; x < 6; x++) {
        let ingredientsId = "ingredientsId" + x.toString();
        let ingredientsList = "ingredientsList" + x.toString();
        $("#" + ingredientsList).empty();
        $("#" + ingredientsId).on("input", function () {
            let options = {};
            options.url = "/Add/GetIngredients";
            options.type = "GET";
            options.data = { "ingredient": $("#" + ingredientsId).val() };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + ingredientsList).empty();
                for (let m = 0; m < data.length; m++) {
                    $("#" + ingredientsList).append("<option value='" + data[m].trim() + "'></option> ");
                }
            };
            $.ajax(options);
        });
    }
});


// fires up functions to monitor the 5 initial Ingredients fields and
// use ajax calls to get either the Dry or Wet Units of Measure based upon the ingredient
// for drop-down select menu
$(document).ready(function () {
    for (let y = 1; y < 6; y++) {
        let ingredientsId = "ingredientsId" + y.toString();
        let ingredientsUOMId = "ingredientsUOMId" + y.toString();
        $("#" + ingredientsId).on("input", function () {
            $("#" + ingredientsUOMId).empty();
            let options = {};
            options.url = "/Add/GetUnitsOfMeasureAdd";
            options.type = "GET";
            options.data = { "ingredient": $("#" + ingredientsId).val() };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + ingredientsUOMId).empty();
                for (let m = 0; m < data.length; m++) {
                    var opt = document.createElement('option');
                    opt.value = data[m].trim();
                    opt.innerHTML = data[m].trim();
                    $("#" + ingredientsUOMId).append(opt);
                }
            };
            $.ajax(options);
        });
    }
});


/*
// monitor required fields and remove red highlighting when user inputs data
$("$recipeForm :input").change(function () {
    checkAddRecipeFields();
});
*/

// enables the 'Add Ingredient Row' button once the existing 5 ingredient fields have been used
$(document).ready(function () {
    var populated = [false, false, false, false, false];
    for (let w = 1; w < 6; w++) {
        let ingredientsId = "ingredientsId" + w.toString();
        $("#" + ingredientsId).on("input", function () {
            let ingredientsIdVal = $("#" + ingredientsId).val;
            if (ingredientsIdVal) {
                populated[w - 1] = true;
            }
            for (v = 0; v < populated.length; v++) {
                var count = 0;
                for (p = 0; p < populated.length; p++) {
                    if (populated[p].toString().localeCompare('true') == 0) {
                        count++;
                    }
                }
                if (count == 5) {
                    document.getElementById('newIngredientButton').disabled = false;
                }
                else {
                    document.getElementById('newIngredientButton').disabled = true;
                }
            }
        });
    }
});


// generates new Ingredient, Amount, Unit of Measure and Comment fields when the
// 'Add Ingredient Row' button is clicked
ingredCounter = 5;
function addIngredient() {
    ingredCounter++;
    var insertNewIngredientId = document.getElementById('newIngredient');
    let container = document.createElement("container");
    var newContainerId = "ingredientsIdContainer" + ingredCounter; 
    container.id = newContainerId;
    container.classList.add('d-flex');
    container.classList.add('flex-row');
    container.classList.add('align-content-stretch');
    insertNewIngredientId.append(container);
    var newId = "ingredientsId" + ingredCounter;
    var newList = "ingredientsList" + ingredCounter;
    var ingredientsUOMId = "ingredientsUOMId" + ingredCounter;
    let newFieldSet = document.createElement('fieldset');
    newFieldSet.classList.add("p-3");
    container.appendChild(newFieldSet);
    let newInput = document.createElement('input');
    //let newLegend = document.createElement('legend');    
    //newLegend.innerHTML = 'Ingredient:';
    //newFieldSet.append(newLegend);
    newInput.setAttribute("id", newId);
    newInput.setAttribute("list", newList);
    newFieldSet.append(newInput);
    let newDataList = document.createElement('datalist');
    newDataList.id = newList;
    newFieldSet.append(newDataList);

    $(document).ready(function () {
        $("#" + newId).on("input", function () {
            let options = {};
            options.url = "/Add/GetIngredients";
            options.type = "GET";
            options.data = { "ingredient": $("#" + newId).val() };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + newList).empty();
                for (let m = 0; m < data.length; m++) {
                    $("#" + newList).append("<option value='" + data[m].trim() + "'></option> ");
                }
            };
            $.ajax(options);
        });
    });

    // Amount field
    let newFieldSet2 = document.createElement('fieldset');
    newFieldSet2.classList.add("p-3");
    container.appendChild(newFieldSet2);
    var newAmountId = "ingredientsAmountId" + ingredCounter;
    //let amountLegend = document.createElement('legend');
    //amountLegend.innerHTML = 'Amount:';
    //newFieldSet2.append(amountLegend);
    let amountInput = document.createElement('input');
    amountInput.setAttribute("id", newAmountId);
    amountInput.setAttribute("name", "quantity");
    amountInput.type = "number";
    amountInput.min = "1";
    amountInput.max = "1000000";
    amountInput.placeholder = "0";
    amountInput.innerHTML = "Amount";
    newFieldSet2.append(amountInput);

    // Unit Of measure field drop-down
    let newFieldSet3 = document.createElement('fieldset');
    newFieldSet3.classList.add("p-2");
    container.appendChild(newFieldSet3);
    var newUOMId = "ingredientsUOMId" + ingredCounter;
    //let newLegend3 = document.createElement('legend');
    //newLegend3.innerHTML = 'Unit Of Measure:';
    //newFieldSet3.append(newLegend3);
    let newSelect = document.createElement('select');
    newSelect.setAttribute("id", newUOMId);
    newSelect.setAttribute("name", newUOMId);
    newSelect.placeholder = "--optional--";
    newSelect.style = "width:135px";
    newFieldSet3.append(newSelect);

     // the JS function to get Dry / Wet units of measure options
    // when an ingredient is selected
    $("#" + newId).on("input", function () {
        $("#" + ingredientsUOMId).empty();
        let options = {};
        options.url = "/Add/GetUnitsOfMeasureAdd";
        options.type = "GET";
        options.data = { "ingredient": $("#" + newId).val() };
        options.dataType = "json";
        options.async = true;
        options.success = function (data) {
            $("#" + newUOMId).empty();
            for (let m = 0; m < data.length; m++) {
                var opt = document.createElement('option');
                opt.value = data[m].trim();
                opt.innerHTML = data[m].trim();
                $("#" + newUOMId).append(opt);
            }
        };
        $.ajax(options);
    });

    // Note
    let newFieldSet4 = document.createElement('fieldset');
    newFieldSet4.classList.add("p-3");
    container.appendChild(newFieldSet4);
    var newCommentId = "ingredientsCommentId" + ingredCounter;
    //let noteLegend = document.createElement('legend');
    //noteLegend.innerHTML = 'Note:';
    //newFieldSet4.append(noteLegend);
    let newtextarea = document.createElement('textarea');
    newtextarea.setAttribute("id", newCommentId);
    newtextarea.setAttribute("name", newCommentId);
    newtextarea.cols = "50";
    newtextarea.placeholder = "--optional--";
    newFieldSet4.append(newtextarea);
}


// bring newly added Ingredient elements into view
function positionNewIngredient(){
    let elem = document.getElementById('newIngredientButton');
    elem.scrollIntoView();
    var fields = ["newIngredientSingularId", "newIngredientPluralId", "newIngredientFoodCategoryId",
                    "dryOrWetMeasureId", "addIngredientButton", "closeAddIngredientFormButton"];
    fields.forEach(function(item, index) {
        positionEachNewIngredient(item);
    });
}


// add a 'heartBeat' effect to the new Ingredient element fields
function positionEachNewIngredient(ingredient)
{
    let elemt = document.getElementById(ingredient);
    elemt.classList.add('wow', 'heartBeat', 'ingredientElement');    
    elemt.setAttribute('data-wow-duration', '2s');
}


// fires up functions to monitor the 5 initial Instruction Step fields and
// use ajax calls to get the Temperature Units of Measure for drop-down select menu
$(document).ready(function () {
    var tempUOMs;
    let options = {};
    options.url = "/Add/GetTemperatureUnitsOfMeasure";
    options.type = "GET";
    options.dataType = "json";
    options.async = true;
    options.success = function (data) {
        tempUOMs = data;
    };
    $.ajax(options);

    for (let g = 1; g < 6; g++) {
        let instructionTempId = "instructionTempId" + g.toString();
        let instructionTempUoMId = "instructionTempUoMId" + g.toString();
        $("#" + instructionTempId).on("input", function () {
            $("#" + instructionTempUoMId).empty();    
            for (let m = 0; m < tempUOMs.length; m++) {
                var opt = document.createElement('option');
                opt.value = tempUOMs[m].trim();
                opt.innerHTML = tempUOMs[m].trim();
                $("#" + instructionTempUoMId).append(opt);
            }          
        });
    }
});


// enables the 'Add Instruction Row' button once the existing 5 instruction rows have been used
$(document).ready(function () {
    var populated = [false, false, false, false, false];
    for (let c = 1; c < 6; c++) {
        let instructionId = "instructionId" + c.toString();
        $("#" + instructionId).on("input", function () {
            let instructionIdVal = $("#" + instructionId).val;
            if (instructionIdVal) {
                populated[c - 1] = true;
            }
            for (let v = 0; v < populated.length; v++) {
                var count = 0;
                for (p = 0; p < populated.length; p++) {
                    if (populated[p].toString().localeCompare('true') == 0) {
                        count++;
                    }
                }
                if (count == 5) {
                    document.getElementById('newInstructionButton').disabled = false;
                }
                else {
                    document.getElementById('newInstructionButton').disabled = true;
                }
            }
        });
    }
});


// generates new Instruction Row with optional temperature and temperature unit of measure fields
instruCounter = 5;
function addNewInstructionRow() {
    instruCounter++;
    var insertNewInstructionId = document.getElementById('newInstruction');
    let container = document.createElement("container");
    var newContainerId = "instructionIdContainer" + ingredCounter;
    container.setAttribute("id", newContainerId);
    container.classList.add('d-flex');
    container.classList.add('flex-row');
    container.classList.add('align-content-stretch');
    insertNewInstructionId.append(container);
    var newId = "instructionId" + instruCounter;
    var newName = "instructionName" + instruCounter;
    let element = document.getElementById('newInstruction');
    let newFieldSet = document.createElement('fieldset');
    newFieldSet.classList.add("p-3");
    container.appendChild(newFieldSet);
    let newTextarea = document.createElement('textarea');
    //let newLegend = document.createElement('legend');    
    //newLegend.innerHTML = 'Instruction:';
    //newFieldSet.append(newLegend);
    newTextarea.setAttribute("id", newId);
    newTextarea.setAttribute("name", newName);
    newTextarea.setAttribute("class", "clearDefault");
    newTextarea.setAttribute("rows", "1");
    newTextarea.setAttribute("cols", "50");
    let placeholderValue = "-- Step " + instruCounter + "--";
    newTextarea.setAttribute("placeholder", placeholderValue);
    newFieldSet.append(newTextarea);

    // Temperature field
    let newFieldSet2 = document.createElement('fieldset');
    newFieldSet2.classList.add("p-3");
    container.appendChild(newFieldSet2);
    var newTempId = "instructionTempId" + instruCounter;
    var newTempName = "instructionTempName" + instruCounter;
    //let temperatureLegend = document.createElement('legend');
    //temperatureLegend.innerHTML = 'Temperature (if required):';
    //newFieldSet2.append(temperatureLegend);
    let temperatureInput = document.createElement('input');
    temperatureInput.setAttribute("id", newTempId);
    temperatureInput.setAttribute("name", newTempName);
    temperatureInput.type = "number";
    temperatureInput.min = "1";
    temperatureInput.max = "100";
    temperatureInput.placeholder = "--optional--";
    newFieldSet2.append(temperatureInput);

    // Temperature Unit of Measure drop-down field
    let newFieldSet3 = document.createElement('fieldset');
    newFieldSet3.classList.add("p-3");
    container.appendChild(newFieldSet3);
    var newTempUoMId = "instructionTempUoMId" + instruCounter;
    var newTempUoMName = "instructionTempUoMName" + instruCounter;
    //let newLegend3 = document.createElement('legend');
    //newLegend3.innerHTML = 'Temperature Unit(if required)';
    //newFieldSet3.append(newLegend3);
    let newSelect = document.createElement('select');
    newSelect.setAttribute("id", newTempUoMId);
    newSelect.setAttribute("name", newTempUoMName);
    newSelect.style = "width:135px";
    newFieldSet3.append(newSelect);

    // the JS function to get the temperature unit of measure 
    // if a temperature value is entered
    let instructionTempId = "instructionTempId" + instruCounter.toString();
    let instructionTempUoMId = "instructionTempUoMId" + instruCounter.toString();
    $("#" + instructionTempId).on("input", function () {
        $("#" + instructionTempUoMId).empty();
        let options = {};
        options.url = "/Add/GetTemperatureUnitsOfMeasure";
        options.type = "GET";
        options.dataType = "json";
        options.async = true;
        options.success = function (data) {
            $("#" + instructionTempUoMId).empty();
            for (let m = 0; m < data.length; m++) {
                var opt = document.createElement('option');
                opt.value = data[m].trim();
                opt.innerHTML = data[m].trim();
                $("#" + instructionTempUoMId).append(opt);
            }
        };
        $.ajax(options);
    });
}


// clears default values (eg '-- optional --') from input fields
$(document).ready(function () {
    $('.clearDefault').on("focus", function () {
        if (this.innerHTML.val === this.defaultValue) {
            this.value = '';
        }
    });
});


// displays a thumbnail of the uploaded recipe image
var displayimage = function(event){
    var image = document.getElementById('fileoutput');
    image.src = URL.createObjectURL(event.target.files[0]);
};


// check fields and add recipe
function processRecipe() {
    // clear fields
    prepareRecipeFeedbackDiv();

    if (checkAddRecipeFields() == 0) {
        var input = document.getElementById('recipeimage').files[0];
        if (input != null) {
            var formData = new FormData();
            formData.append("file", input);
            $.ajax(
                {
                    url: "/Add/ProcessNewImage",
                    data: formData,
                    processData: false,
                    contentType: false,
                    async: true,
                    type: "POST",
                    success: function (RecipeIdent) {
                        processRecipeFields(RecipeIdent);
                    },
                    error: function () {
                        var updateMessage = "Recipe not saved; please contact System Administrator";
                        alertify.alert('Error', updateMessage, function () { });
                    },
                }
            );
        } else  // no recipe image
        {
            processRecipeFields("none");
        }
    } else {
        requiredFieldsPrompt(); 
    }
}


function processRecipeFields(RecipeIdent) {
    // disable the Add Recipe buttom   
    document.getElementById('addRecipeButton').disabled = true;

    // prep variables to be used
    var ingredientsName = [];
    var ingredientsValue = [];
    var amountsName = [];
    var amountsValue = [];
    var uomName = [];
    var uomVals = [];
    var commentsName = [];
    var commentsValues = [];

    if (document.getElementById('trueFreeze').checked) {
        freezeVal = true;
    }
    if (document.getElementById('falseFreeze').checked) {
        freezeVal = false;
    }

    var courseNames = [];
    var meal1 = document.getElementById('breakfast').checked;
    if (meal1 == true) { courseNames.push('Breakfast'); }
    var meal2 = document.getElementById('lunch').checked;
    if (meal2 == true) { courseNames.push('Lunch'); }
    var meal3 = document.getElementById('dinner').checked;
    if (meal3 == true) { courseNames.push('Dinner'); }
    var meal4 = document.getElementById('other').checked;
    if (meal4 == true) { courseNames.push('Other'); }

    var courseValues = "";
    for (var x = 0; x < courseNames.length; x++) {
        courseValues = courseValues.concat(courseNames[x] + "|"); 
    }

    for (a = 1; a < ingredCounter + 1; a++) {

        let itemName = "ingredientsId" + a;
        ingredientsName.push(itemName);
        let itemValue = document.getElementById(itemName).value;
        if (itemValue) {  // is not empty
            ingredientsValue.push(itemValue);

            let amount = "ingredientsAmountId" + a;
            amountsName.push(amount);
            let amountVal = document.getElementById(amount).value;
            amountsValue.push(amountVal);

            let uom = "ingredientsUOMId" + a;
            uomName.push(uom);
            let uomVal = document.getElementById(uom).value;
            uomVals.push(uomVal);

            let note = "ingredientsCommentId" + a;
            commentsName.push(note);
            let commentsVals = document.getElementById(note).value;
            commentsValues.push(commentsVals);
        }
    }

    var instructionsName = [];
    var instructionsValue = [];
    var instructionTempName = [];
    var instructionTempValue = [];
    var originalTemperatureUoMName = [];
    var originalTemperatureUoMNameVal = [];

    for (var b = 1; b < instruCounter + 1; b++) {

        let itemName2 = "instructionId" + b;
        instructionsName.push(itemName2);
        let itemValue2 = document.getElementById(itemName2).value;
        if (itemValue2) {
            instructionsValue.push(itemValue2);
            let tempName = "instructionTempId" + b;
            instructionTempName.push(tempName);
            let tempVal = document.getElementById(tempName).value;
            instructionTempValue.push(tempVal);
            let unitName = "instructionTempUoMId" + b;
            originalTemperatureUoMName.push(unitName);
            let unitVal = document.getElementById(unitName).value;
            originalTemperatureUoMNameVal.push(unitVal);
        }
    }

    let options = {};
    options.url = "/Add/ProcessNewRecipe";
    options.type = "POST";
    options.async = true;
    options.data =
    {
        "recipeName": $("#" + "recipeName").val(),
        "description": $("#" + "description").val(),
        "course": courseValues,
        "cookingMethod": $("#" + "cookingMethod").val(),
        "servings": $("#" + "servings").val(),
        "cookingTime": $("#" + "cookingTime").val(),
        recipeIdt : RecipeIdent,
        ingredientsName: ingredientsValue,
        amountsName: amountsValue,
        uomName: uomVals,
        commentsName: commentsValues,
        instructionsName: instructionsValue,
        instructionTempName: instructionTempValue,
        originalTemperatureUoMName: originalTemperatureUoMNameVal,
        "additionalNotes": $("#" + "additionalNotes").val(),
        "goesWithNotes": $("#" + "goesWithNotes").val(),
        "season": $("#" + "season").val(),
        "cuisine": $("#" + "cuisine").val(),
        "anevent": $("#" + "anevent").val(),
        "dietaryRequirement": $("#" + "dietaryRequirement").val(),
        "freezable": freezeVal,
        "mainFoodCategory": $("#" + "mainFoodCategory").val(),
    };

    options.success = function () {
        var rname = document.getElementById('recipeName').value;
        var updateMessage = "Recipe '" + rname + "' saved!";
        alertify.alert('Success', updateMessage, function () { });
    };
    options.error = function () {
        var updateMessage = "Recipe saving process did not complete successfully.";
        alertify.alert('Error', updateMessage, function () { });
        let options = {};
        options.url = "/Add/DeleteImageOrphans";
        options.type = "GET";
        options.async = true;
        options.data = {};
        $.ajax(options);
    };
    $.ajax(options);
} 


// prepare elements for feedback updates to user
function prepareRecipeFeedbackDiv() {
    var recipeFeedbackDivId = document.getElementById('recipeFeedbackDivId');
    var recipeFeedbackPara = document.createElement('paragraph');
    recipeFeedbackPara.id = 'recipeFeedbackParaId';
    recipeFeedbackPara.classList.add('hide');
    recipeFeedbackDivId.appendChild(recipeFeedbackPara);
    recipeFeedbackDivId.classList.add('hide');
}


// prompt user to complete all required recipe fields
function requiredFieldsPrompt() {
    let outputDiv = document.getElementById('recipeFeedbackDivId');
    outputDiv.classList.remove('hide');
    outputDiv.classList.add('show');
    outputDiv.style.height = '40px';
    outputDiv.style.marginTop = '10px';
    outputDiv.style.color = '#ff0000';
    outputDiv.style.fontSize = '25px';
    let outputPara = document.getElementById('recipeFeedbackParaId');
    outputPara.classList.remove('hide');
    outputPara.classList.add('show');
    outputPara.style.visibility = "visible";
    outputPara.innerHTML = "Please Complete Required Fields and Check Input";
    let mainHeader = document.getElementById('mainHeader');
    mainHeader.scrollIntoView();
    // mainHeader.style.marginTop = '145px';
}


// check whether required Recipe fields have been populated
function checkAddRecipeFields() {
    var populatedFields = 0;
    var fieldIds = ['recipeName', 'ingredientsId1', 'ingredientsUOMId1', 'instructionId1'];

    // remove previous validation
    var rangeIds = ['recipeName', 'courseDiv', 'cookingmethodDiv', 'cookingTimeDiv', 'servingsDiv', 'freezeDiv',
        'mainFoodCatDiv', 'ingredientsId1', 'ingredientsAmountId1', 'ingredientsUOMId1', 'instructionId1'];
    rangeIds.forEach(removeInvalidation);

    // Course validation
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var checkedOne = Array.prototype.slice.call(checkboxes).some(x => x.checked);
    if (checkedOne == false) {
        populatedFields++;
        var courseId = document.getElementById('courseDiv');
        processClasses(courseId);
    }

    // Cooking Method validation
    var cookMethod = document.getElementById('cookingMethod').value;
    if ((cookMethod.length == 0) || (cookMethod == "blank")) {
        populatedFields++;      
        var cookMethod2 = document.getElementById('cookingmethodDiv');
        processClasses(cookMethod2);
    }   

    // Cooking Time validation
    var cookTime = document.getElementById('cookingTime').value;
    if ((cookTime.length == 0) || (cookTime == "blank")) {
        populatedFields++;        
        var cookingTimeId = document.getElementById('cookingTimeDiv');
        processClasses(cookingTimeId);
    }   

    // Servings validation
    var servingsvalue = document.getElementById('servings').value;
    if ((servingsvalue < 1) || (servingsvalue > 100)) {
        populatedFields++;         
        var servingsdiv = document.getElementById('servingsDiv');
        processClasses(servingsdiv);
    }   

    // Freezable validation
    var freeze = $("input[type='radio'][name='freezable']:checked").val();
    if ((freeze != "Yes") && (freeze != "No")) {
        populatedFields++;                 
        var freezediv = document.getElementById('freezeDiv');
        processClasses(freezediv);
    }   

    // Main Food Category validation
    var mainFC = document.getElementById('mainFoodCategory').value;
    if ((mainFC == 0) || (mainFC == "blank")) {
        populatedFields++;               
        var mainFoodCatDiv = document.getElementById('mainFoodCatDiv');
        processClasses(mainFoodCatDiv);
    }  

    if (document.getElementById('ingredientsAmountId1').value < 1) {
        populatedFields++;               
        var amount = document.getElementById('ingredientsAmountId1');
        processClasses(amount);
    }

    fieldIds.forEach(function (item) {
        populatedFields += checkEachRecipeField(item);
    });

    // check ingredient amount values, where used
    for (var i = 1; i < ingredCounter; i++) {
        populatedFields += checkIngredAmountRange(i);
    }

    // check instruction temperature values, where used
    for (var n = 1; n < instruCounter; n++) {
        populatedFields += checkInstructTempRange(n);
    }
    return populatedFields;
}


function checkIngredAmountRange(ingred) {
    var populated = $('#ingredientsAmountId' + ingred).val();
    if (populated != 0) {
        if ((populated > 0) && (populated <= 1000000)) {
            return 0;
        } else {
            return 1;
        }
    }
    else {
        return 0;
    }
}


function checkInstructTempRange(instruct) {
    var populated = $('#instructionTempId' + instruct).val();
    if (populated != 0) {
        if ((populated > 0) && (populated <= 999)) {
            return 0;
        } else {
            return 1;
        }
    }
    else {
        return 0;
    }
}


function processClasses(fieldClass) {
    fieldClass.classList.add('invalidRequired');
    fieldClass.classList.remove('heartbeat');
    fieldClass.classList.add('rubberBand');
}


function removeInvalidation(item, index) {
    var input = document.getElementById(item);
    input.classList.remove('invalidRequired');
}


// check whether an individual Recipe field is populated, 
// if not use heatBeat to draw user's attention
function checkEachRecipeField(fieldId) {
    var notPopulated = 0;
    var eleVId = document.getElementById(fieldId);
    if (eleVId.value.length == 0) {
        eleVId.classList.add('invalidRequired');
        notPopulated = 1;
        eleVId.classList.remove('heartbeat');
        eleVId.classList.add('rubberBand');
        console.log(fieldId + " not vlaidated"); //////////////////////////////////////
    }
    return notPopulated;
}


$(document).ready(function () {
	//override Alertify.js defaults to look like Bootstrap
	alertify.defaults.transition = "slide";
	alertify.defaults.theme.ok = "btn btn-primary";
	alertify.defaults.theme.cancel = "btn btn-danger";
	alertify.defaults.theme.input = "form-control";

	wow = new WOW(
	{
		boxClass:     'wow',      
		animateClass: 'animated', 
		offset:       0,          
        mobile:       true,       
		live:         true        
	});
	wow.init();
});

$('selector').click(function (event) {
	if (!event.detail || event.detail == 1) {
		$('selector').disable = true;
	}
});

