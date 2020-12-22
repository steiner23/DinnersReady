// Uses ajax calls to get either the Dry or Wet Units of Measure based upon the ingredient
// for drop-down select menu and then to select the the Original Unit of Measure
// For Instructions, loads the Temperature Units of Measure and selects
// the Original Unit of Measure
initialIngredCounter = $('.ingredient').length;
initialInstructCounter = $('.instruction').length;
deletes = [];
added = [];

$(document).ready(function ()
{
    var ingredCounter = $('.ingredient').length;
    var instructCounter = $('.instruction').length;

    for (var y = 1; y <= ingredCounter; y++) {
        var ingredientsId = "ingredientsVId" + y.toString();
        var ingredientsUOMId = "ingredientsVUOMId" + y.toString();
        var ingredientsUOMIdHidden = "ingredientsVUOMIdOld" + y.toString();
        var ingredientsUOMIdHiddenValue = $("#" + ingredientsUOMIdHidden).val();

        $("#" + ingredientsUOMId).empty();
        var options = {};
        options.url = "/Edit/GetUnitsOfMeasureView";
        options.type = "GET";
        options.data = { "ingredient": $("#" + ingredientsId).val() };
        options.dataType = "json";
        options.async = false;
        options.success = function (data)
        {            
            for (let m = 0; m < data.length; m++) {
                if (data[m].trim() !== "Item ( Items )")
                {
                    var opt = document.createElement('option');
                    opt.value = data[m].trim();
                    opt.innerHTML = data[m].trim();
                    if (data[m].trim() === ingredientsUOMIdHiddenValue.trim()) {
                        opt.selected = true;
                    } // end if
                    $("#" + ingredientsUOMId).append(opt);
                } // end if
            } // end for   
        }; // end function
        $.ajax(options);
    } // end for
    
    var tempUOMs;

    var options2 = {};
    options2.url = "/Edit/GetTemperatureUnitsOfMeasure";
    options2.type = "GET";
    options2.dataType = "json";
    options2.async = false;
    options2.success = function (data) {
        tempUOMs = data;
    };
    $.ajax(options2);

    for (var z = 1; z <= instructCounter; z++) {
        var instructionTempUoMId = "instructionVTempUoMId" + z.toString();
        var instructionTempUoMIdHidden = "instructionVTempUOMIdOld" + z.toString();
        var instructionTempUoMIdHiddenValue = $("#" + instructionTempUoMIdHidden).val();
        $("#" + instructionTempUoMId).empty();

        if (instructionTempUoMIdHiddenValue.trim() == "--none--") {
            var opt = document.createElement('option');
            opt.value = "--none--";
            opt.innerHTML = "--none--";
            opt.selected = true;
            $("#" + instructionTempUoMId).append(opt);                    
        }
        else {
            for (var m = 0; m < tempUOMs.length; m++) {
                var opt2 = document.createElement('option');
                opt2.value = tempUOMs[m].trim();
                opt2.innerHTML = tempUOMs[m].trim();
                if (tempUOMs[m].trim() === instructionTempUoMIdHiddenValue.trim()) {
                    opt2.selected = true;
                } // end if
                $("#" + instructionTempUoMId).append(opt2);
            } // end for
        } // end else            
    } // end for

    // move page up to main header
    document.getElementById('body').scrollIntoView();
});


// monitor: fires up functions to monitor the Ingredients fields and
// use ajax calls to get Ingredient name suggestions from the database
$(document).ready(function () {
    var ingredCounter = $('.ingredient').length;
    for (var x = 1; x < ingredCounter; x++) {
        var ingredientsId = "ingredientsId" + x.toString();
        var ingredientsList = "ingredientsList" + x.toString();
        $("#" + ingredientsList).empty();
        $("#" + ingredientsId).on("input", function () {
            let options = {};
            options.url = "/Edit/GetIngredients";
            options.type = "GET";
            options.data = { "ingredient": $("#" + ingredientsId).val() };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + ingredientsList).empty();
                for (var m = 0; m < data.length; m++) {
                    $("#" + ingredientsList).append("<option value='" + data[m].trim() + "'></option> ");
                }
            };
            $.ajax(options);
        });
    }
});


// generates new Ingredient, Amount, Unit of Measure, Comment fields and Remove button
// when the 'Add Ingredient Row' button is clicked
function addIngredient() {
    ingredCounter = initialIngredCounter;
    initialIngredCounter++;
    ingredCounter++;  
    var insertNewIngredientId = document.getElementById('newIngredientDiv');
    insertNewIngredientId.classList.remove('hide');
    insertNewIngredientId.classList.add('show');    
    var container = document.createElement("container");
    var newContainerId = "ingredientsIdContainer" + ingredCounter;
    added.push(newContainerId);
    container.setAttribute("id", newContainerId);
    container.classList.add('d-flex');
    container.classList.add('flex-row');
    container.classList.add('align-content-stretch');
    insertNewIngredientId.append(container);
    var newId = "ingredientsVId" + ingredCounter;
    var newList = "ingredientsVList" + ingredCounter;
    var newAmountId = "ingredientsVAmountId" + ingredCounter;
    var ingredientsUOMId = "ingredientsVUOMId" + ingredCounter;
    var newFieldSet = document.createElement('fieldset');
    newFieldSet.classList.add("p-3");
    container.appendChild(newFieldSet);
    //var newLegend = document.createElement('legend');
    //newLegend.innerHTML = 'Ingredient:';
    //newFieldSet.append(newLegend);
    var newInput = document.createElement('input');    
    newInput.setAttribute("id", newId);
    newInput.setAttribute("class", "ingredient");
    newInput.setAttribute("list", newList);
    newFieldSet.append(newInput);
    var newDataList = document.createElement('datalist');
    newDataList.setAttribute("id", newList);
    newFieldSet.append(newDataList);

    $(document).ready(function () {
        $("#" + newId).on("input", function () {
            var options = {};
            options.url = "/Edit/GetIngredients";
            options.type = "GET";
            options.data = { "ingredient": $("#" + newId).val() };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + newList).empty();
                for (var m = 0; m < data.length; m++) {
                    $("#" + newList).append("<option value='" + data[m].trim() + "'></option> ");
                }
            };
            $.ajax(options);
        });
    });

    // Amount field
    var newFieldSet2 = document.createElement('fieldset');
    newFieldSet2.classList.add("p-3");
    container.appendChild(newFieldSet2);
    //var amountLegend = document.createElement('legend');
    //amountLegend.innerHTML = 'Amount:';
    //newFieldSet2.append(amountLegend);
    var amountInput = document.createElement('input');
    amountInput.setAttribute("id", newAmountId);
    amountInput.setAttribute("name", "quantity");
    amountInput.type = "number";
    amountInput.min = "1";
    amountInput.max = "1000000";
    amountInput.placeholder = "0";
    amountInput.innerHTML = "Amount";
    newFieldSet2.append(amountInput);

    // Unit Of measure field drop-down
    var newFieldSet3 = document.createElement('fieldset');
    newFieldSet3.classList.add("p-3");
    container.appendChild(newFieldSet3);
    var newUOMId = "ingredientsVUOMId" + ingredCounter;
    //var newLegend3 = document.createElement('legend');
    //newLegend3.innerHTML = 'Unit Of Measure:';
    //newFieldSet3.append(newLegend3);
    var newSelect = document.createElement('select');
    newSelect.setAttribute("id", newUOMId);
    newSelect.setAttribute("name", newUOMId);
    newSelect.style = "width:135px";
    newFieldSet3.append(newSelect);

    // the JS function to get Dry / Wet units of measure options
    // when an ingredient is selected
    $("#" + newId).on("input", function () {
        $("#" + ingredientsUOMId).empty();
        var options = {};
        options.url = "/Edit/GetUnitsOfMeasureView";
        options.type = "GET";
        options.data = { "ingredient": $("#" + newId).val() };
        options.dataType = "json";
        options.async = true;
        options.success = function (data) {
            $("#" + newUOMId).empty();
            for (var m = 0; m < data.length; m++) {
                var opt = document.createElement('option');
                opt.value = data[m].trim();
                opt.innerHTML = data[m].trim();
                $("#" + newUOMId).append(opt);
            }
        };
        $.ajax(options);
    });

    // Note
    var newFieldSet4 = document.createElement('fieldset');
    newFieldSet4.classList.add("p-3");
    container.appendChild(newFieldSet4);
    var newCommentId = "ingredientsVCommentId" + ingredCounter;
    //var noteLegend = document.createElement('legend');
    //noteLegend.innerHTML = 'Note:';
    //newFieldSet4.append(noteLegend);
    var newtextarea = document.createElement('textarea');
    newtextarea.setAttribute("id", newCommentId);
    newtextarea.setAttribute("name", newCommentId);
    newtextarea.cols = "40";
    newtextarea.placeholder = "--optional--";
    newFieldSet4.append(newtextarea);

    // Remove button
    var newFieldSet5 = document.createElement('fieldset');
    newFieldSet5.classList.add("p-1");
    container.appendChild(newFieldSet5);
    //var delLegend = document.createElement('legend');
    //delLegend.innerHTML = 'Delete?';
    //newFieldSet5.append(delLegend);
    var newInput2 = document.createElement('input');
    newInput2.setAttribute('type', 'button');
    newInput2.classList.add('btn');
    newInput2.classList.add('btn-danger');
    newInput2.classList.add('delButton');
    newInput2.setAttribute("value", "X");
    newInput2.setAttribute("onclick", "deleteRow('" + newContainerId + "')" );    
    newFieldSet5.append(newInput2);
}


// generates new Instruction Row  with optional temperture and temperature unit of measure fields
function addNewInstructionRow() {
    instructCounter = initialInstructCounter;
    initialInstructCounter++;
    instructCounter++;
    var insertNewInstruction = document.getElementById('newInstructionDiv');
    var container = document.createElement("container");
    var newContainerId = "instructionIdContainer" + instructCounter;
    added.push(newContainerId);
    container.setAttribute("id", newContainerId);
    container.classList.add('d-flex');
    container.classList.add('flex-row');
    container.classList.add('align-content-stretch');
    insertNewInstruction.append(container);
    var newId = "instructionVId" + instructCounter;
    var newName = "instructionVName" + instructCounter;
    var newFieldSet = document.createElement('fieldset');
    newFieldSet.classList.add("instruction");
    newFieldSet.classList.add("p-6");
    container.appendChild(newFieldSet);
    //var newLegend = document.createElement('legend');   
    //newLegend.innerHTML = 'Instruction:';
    //newFieldSet.append(newLegend);
    var newTextarea = document.createElement('textarea');     
    newTextarea.setAttribute("id", newId);
    newTextarea.setAttribute("name", newName);
    newTextarea.setAttribute("class", "instruction");
    newTextarea.setAttribute("class", "clearDefault");
    newTextarea.setAttribute("rows", "1");
    newTextarea.setAttribute("cols", "50");
    var placeholderValue = "-- Step --";
    newTextarea.setAttribute("placeholder", placeholderValue);
    newFieldSet.append(newTextarea);

    // Temperature field
    var newFieldSet2 = document.createElement('fieldset');
    newFieldSet2.classList.add("p-2");
    container.appendChild(newFieldSet2);
    var newTempId = "instructionVTempId" + instructCounter;
    var newTempName = "instructionVTempName" + instructCounter;
    //var temperatureLegend = document.createElement('legend');
    //temperatureLegend.innerHTML = 'Temperature (if required):';
    //newFieldSet2.append(temperatureLegend);
    var temperatureInput = document.createElement('input');
    temperatureInput.setAttribute("id", newTempId);
    temperatureInput.setAttribute("name", newTempName);
    temperatureInput.type = "number";
    temperatureInput.min = "1";
    temperatureInput.max = "500";
    temperatureInput.placeholder = "--optional--";
    newFieldSet2.append(temperatureInput);

    // Temperature Unit of Measure drop-down field
    var newFieldSet3 = document.createElement('fieldset');
    newFieldSet3.classList.add("p-3");
    container.appendChild(newFieldSet3);
    var newTempUoMId = "instructionVTempUoMId" + instructCounter;
    var newTempUoMName = "instructionVTempUoMName" + instructCounter;
    //var newLegend3 = document.createElement('legend');
    //newLegend3.innerHTML = 'Temperature Unit(if required)';
    //newFieldSet3.append(newLegend3);
    var newSelect = document.createElement('select');
    newSelect.setAttribute("id", newTempUoMId);
    newSelect.setAttribute("name", newTempUoMName);
    newSelect.setAttribute("placeholder", "optional");
    newSelect.classList.add('clearDefault');
    newSelect.style = "width:135px";
    newFieldSet3.append(newSelect);

    // Remove button
    var newFieldSet4 = document.createElement('fieldset');
    newFieldSet4.classList.add("p-1");
    container.appendChild(newFieldSet4);
    //var newLegend4 = document.createElement('legend');
    //newLegend4.innerHTML = 'Delete?';
    //newFieldSet4.append(newLegend4);
    var newInput = document.createElement('input');
    newInput.setAttribute('type', 'button');
    newInput.classList.add('instructDeletes');
    newInput.classList.add('btn');
    newInput.classList.add('btn-danger');
    newInput.classList.add('delButton');
    newInput.setAttribute("value", "X");
    newInput.setAttribute("onclick", "deleteRow('" + newContainerId + "')" );
    newFieldSet4.append(newInput);

    // the JS function to get the temperature unit of measure
    // if a temperature value is entered
    var instructionTempId = "instructionVTempId" + instructCounter.toString();
    var instructionTempUoMId = "instructionVTempUoMId" + instructCounter.toString();
    $("#" + instructionTempId).on("input", function () {
        $("#" + instructionTempUoMId).empty();
        var options = {};
        options.url = "/Edit/GetTemperatureUnitsOfMeasure";
        options.type = "GET";
        options.dataType = "json";
        options.async = false;
        options.success = function (data) {
            $("#" + instructionTempUoMId).empty();
            for (var m = 0; m < data.length; m++) {
                var opt = document.createElement('option');
                opt.value = data[m].trim();
                opt.innerHTML = data[m].trim();
                $("#" + instructionTempUoMId).append(opt);
            }
        };
        $.ajax(options);
    });  
}


// bring a newly added Ingredient elements into view
function positionNewIngredient() {
    var elem = document.getElementById('newIngredientButton');
    elem.scrollIntoView();
    var fields = ["newIngredientSingularId", "newIngredientPluralId", "newIngredientFoodCategoryId",
        "dryOrWetMeasureId", "addIngredientButton", "closeAddIngredientFormButton"];
    fields.forEach(function (item, index) {
        positionEachNewIngredient(item);
    });
}


// add a 'heartBeat' effect to the new Ingredient element fields
function positionEachNewIngredient(ingredient) {
    var elemt = document.getElementById(ingredient);
    elemt.classList.add('wow', 'heartBeat', 'ingredientElement');
    elemt.setAttribute('data-wow-duration', '2s');
}

// fires up functions to monitor the Ingredients fields and
// use ajax calls to get Ingredient name suggestions from the database
$(document).ready(function () {
    var ingredCounter = $('.ingredient').length;
    for (var x = 1; x < ingredCounter; x++) {
        var ingredientsId = "ingredientsVId" + x.toString();
        var ingredientsList = "ingredientsVList" + x.toString();
        $("#" + ingredientsList).empty();
        $("#" + ingredientsId).on("input", function () {
            let options = {};
            options.url = "/Edit/GetIngredients";
            options.type = "GET";
            options.data = { "ingredient": $("#" + ingredientsId).val() };
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + ingredientsList).empty();
                for (var m = 0; m < data.length; m++) {
                    $("#" + ingredientsList).append("<option value='" + data[m].trim() + "'></option> ");
                }
            };
            $.ajax(options);
        });
    }
});


// fires up functions to monitor the Instruction Step fields and
// use ajax calls to get the Temperature Units of Measure for drop-down select menu
$(document).ready(function () {
    var instructCounter = $('.instruction').length;
    for (var g = 1; g < instructCounter; g++) {
        var instructionTempId = "instructionVTempId" + g.toString();
        var instructionTempUoMId = "instructionVTempUoMId" + g.toString();

        $("#" + instructionTempId).on("input", function () {
            $("#" + instructionTempUoMId).empty();
            let options = {};
            options.url = "/Edit/GetTemperatureUnitsOfMeasure";
            options.type = "GET";
            options.dataType = "json";
            options.async = true;
            options.success = function (data) {
                $("#" + instructionTempUoMId).empty();
                for (var m = 0; m < data.length; m++) {
                    var opt = document.createElement('option');
                    opt.value = data[m].trim();
                    opt.innerHTML = data[m].trim();
                    $("#" + instructionTempUoMId).append(opt);
                }
            };
            $.ajax(options);
        });
    }
});


// displays a thumbnail of the uploaded recipe image
var displayimage = function(event){
    var image = document.getElementById('fileoutput');
    image.src = URL.createObjectURL(event.target.files[0]);
    $('#recipeimageChange').val("true");
};


function valueChange(field){
    var fieldName = field + "Change";
    $('#' + fieldName).val("true");
}


// delete Ingredient or Instruction row
function deleteRow(containerId) {

    if ((containerId.toString().startsWith("ingredientsIdContainer")) || (containerId.toString().startsWith("instructionIdContainer")))
    {                                                                                                     
        deletes.push(containerId);
        // if a row was added since the last save, but is now deleted
        for(var x = 0; x < added.length; x++){
            if(added[x] == containerId){
                var remove = added.splice(x, 1);
                break;
            }
        }
        for(var d = 0; d < deletes.length; d++){
            if(deletes[d] == containerId){
                var remove2 = deletes.splice(d, 1);
                break;
            }
        }   
    }
    else{
        deletes.push(containerId);   
    }  
    document.getElementById(containerId).remove();
}


// process changes to form when 'Save Changes' clicked
function updateRecipe(RecipeIdent) {

    // validate required fields
    var validated = 0;
    validated = checkEditRecipeFields();

    if(validated == 0)
    {
        updateSuccess = true;

        // recipe image
        if( $('#recipeimageChange').val() == "true" ){
        var success = processImage(RecipeIdent);
        if(success == false){ updateSuccess = false; }
        }

        // standard fields
        var success2 = processStandardFields(RecipeIdent);
        if(success2 == false){ updateSuccess = false; }

        // ingredients and instructions 'added' array
        if(added.length > 0){
            success3 = processAddedElements(added, RecipeIdent);
            if(success3 == false){ updateSuccess = false; }
        }

        // ingredients and instructions 'deletes' array
        if(deletes.length > 0){
            var success4 = processDeletedElements(deletes, RecipeIdent);
            if(success4 == false){ updateSuccess = false; } 
        }

        if(updateSuccess == true) {
            var updateMessage = "Recipe " + $("#" + "recipeName").val() + " updated successfully!";
            alertify.alert('Success', updateMessage).set( { onclose:function() {viewAllRecipes();} } );
        }
    }
    else{
        requiredFieldsPrompt();
    }
}


function viewAllRecipes() {
    $.ajax({
        // url: "/List/GetAllRecipes",
        url: "/List/_AllRecipes",
        type: "POST",
        async: true
    }).done(function (data) {
        $("#viewTarget").html(data);
    });
}


function processImage(RecipeIdent) {    

    var inputCheck = document.getElementById('recipeimage').files[0];

    if (inputCheck != null) {
        var input = document.getElementById('recipeimage').files[0];        
        var formData = new FormData();
        formData.append("file", input);      
        formData.append("ident", RecipeIdent);

        $.ajax(
            {
                url: "/Edit/ProcessUpdatedImage",
                data: formData,
                processData: false,
                contentType: false,
                async: true,
                type: "POST",
                success: function (data) { if(data === true) { return "true"; } },
                error: function () {
                    var updateMessage = "Recipe not updated; please contact System Administrator";
                    alertify.alert('Error', updateMessage, function () { });
                    return false;
                },                
            }
        );
        
    }
}


// process updated standard form fields
function processStandardFields(RecipeIdent) {

    var updates = [];

    // remove previous validation
    var rangeIds = ['recipeName', 'courseDiv', 'cookingmethodDiv', 'cookingTimeDiv', 'servingsDiv', 'freezeDiv',
        'mainFoodCatDiv', 'ingredientsVAmountId1', 'ingredientsVId1', 'ingredientsVUOMId1', 'instructionVId1'];
    rangeIds.forEach(removeInvalidation);

    var mainFields = ['recipeName', 'servings', 'description', 'cookingMethod', 'cookingTime', 'mainFoodCategory',
        'season', 'cuisine', 'anevent', 'dietaryRequirement', 'additionalNotes', 'goesWithNotes'];

    for(h = 0; h < mainFields.length; h++){
        if ( $('#' + mainFields[h] + "Change").val() == "true" )
        {
            var n = updates.length;
            updates.push([]);
            updates[n].push( mainFields[h] );
            updates[n].push( $('#' + mainFields[h]).val() );
        }
    }

    var courseRange = ['breakfast', 'lunch', 'dinner', 'other'];
    var courseChanged = false;


    for(var c = 0; c < courseRange.length; c++){
        if( $("#" + courseRange[c] + "Change").val() == true )
        { courseChanged = true; }
    }
    if(courseChanged == true){
        var courseNames = [];
        var meal1 = document.getElementById('breakfast').checked;
        if (meal1 == true) { courseNames.push('Breakfast'); }
        var meal2 = document.getElementById('lunch').checked;
        if (meal2 == true) { courseNames.push('Lunch'); }
        var meal3 = document.getElementById('dinner').checked;
        if (meal3 == true) { courseNames.push('Dinner'); }
        var meal4 = document.getElementById('other').checked;
        if (meal4 == true) { courseNames.push('Other'); }
        var course = "";
        for (var x = 0; x < courseNames.length; x++) {
            course = course.concat(courseNames[x] + "|"); 
        }
        var tempCourse = [];
        tempCourse[0] = "course"; tempCourse[1] = course;
        var n1 = updates.length;
        updates.push([]);
        updates[n1].push( tempCourse[0] );
        updates[n1].push( tempCourse[1] ); 
    }

    if(($('#' + "trueFreeze" + "Change").val() == "true") || ($('#' + "falseFreeze" + "Change").val() == "true"))
    {
        if(  $('#' + 'trueFreeze').is(':checked') )  {
            var p = updates.length;
            updates.push([]);
            updates[p].push( "freeze" ); updates[p].push( "true" );
        }

        if(  $('#' + 'falseFreeze').is(':checked') )  {            
            var r = updates.length;
            updates.push([]);
            updates[r].push( "freeze" ); updates[r].push( "false" );
        }
    }

    if(updates.length > 0){
        let options = {};
        options.url = "/Edit/ProcessUpdatedFields";
        options.type = "POST";
        options.async = true;
        options.data =
        {
            "RecipeIdent" : RecipeIdent,
            "Updates" : updates
        };
        options.success = function (data) { if(data === true) return true; };
        options.error = function () {
            var updateMessage = "Recipe updating process did not complete successfully.";
            alertify.alert('Error', updateMessage, function () { });
            return false;
        };
        $.ajax(options);    
    }
}


function removeInvalidation(item, index) {
    var input = document.getElementById(item);
    input.classList.remove('invalidRequired');
}


function processAddedElements(added, RecipeIdent) {
    if(added.length > 0) {
        var updates = [];
        for(var b = 0; b < added.length; b++) {
            if(added[b].startsWith("ingredients")) {
                var numb = added[b].substring(22, added[b].length);
                var temp = [];
                temp[0] = "Ingredient";
                temp[1] = $("#" + "ingredientsVId" + numb).val();
                temp[2] = $("#" + "ingredientsVAmountId" + numb).val();            
                temp[3] = $("#" + "ingredientsVUOMId" + numb).val();
                temp[4] = $("#" + "ingredientsVCommentId" + numb).val(); 
                var w = updates.length;
                updates.push([]);
                updates[w].push( temp[0] );
                updates[w].push( temp[1] );
                updates[w].push( temp[2] );
                updates[w].push( temp[3] );
                updates[w].push( temp[4] );       
            }else{
                var numb2 = added[b].substring(22, added[b].length);
                var temp2 = [];
                temp2[0] = "Instruction";            
                temp2[1] = $("#" + "instructionVId" + numb2).val();
                temp2[2] = $("#" + "instructionVTempId" + numb2).val();            
                temp2[3] = $("#" + "instructionVTempUoMId" + numb2).val();
                temp2[4] = null;
                var y = updates.length;
                updates.push([]);
                updates[y].push( temp2[0] );
                updates[y].push( temp2[1] );
                updates[y].push( temp2[2] );
                updates[y].push( temp2[3] );
                updates[y].push( temp2[4] );               
            }
        }

        let options = {};
        options.url = "/Edit/ProcessAddedFields";
        options.type = "POST";
        options.async = true;
        options.data =
        {
            "RecipeIdent" : RecipeIdent,
            "Updates" : updates
        };
        options.success = function (data) { if(data === true) return true; };
        options.error = function () {
            var updateMessage = "Recipe updating process did not complete successfully.";
            alertify.alert('Error', updateMessage, function () { });
            return false;
        };
        $.ajax(options);
    } // end if
}


function processDeletedElements(deletes, RecipeIdent) {
    var updates = [];
    for(var d = 0; d < deletes.length; d++) {
        if(deletes[d].startsWith("ingredient")) {        
            var ident = deletes[d].substring(21, deletes[d].length);
            var temp = [];
            temp[0] = "Ingredient";
            temp[1] = ident;
            updates[d] = [];
            updates[d].push( temp[0] );
            updates[d].push( temp[1] );
        } else {
            var ident2 = deletes[d].substring(22, deletes[d].length);
            var temp2 = [];
            temp2[0] = "Instruction";
            temp2[1] = ident2;
            updates[d] = [];
            updates[d].push( temp2[0] );
            updates[d].push( temp2[1] );
        }
    }

    let options = {};
    options.url = "/Edit/ProcessDeletedFields";
    options.type = "POST";
    options.async = true;
    options.data =
    {
        "RecipeIdent" : RecipeIdent,
        "Updates" : updates
    };
    options.success = function (data) { if(data === true) return true; };
    options.error = function () {
        var updateMessage = "Recipe updating process did not complete successfully.";
        alertify.alert('Error', updateMessage, function () { });
        return false;
    };
    $.ajax(options);  
}


// get compare Ingredient row
function processIngredientRow(varIngredNumb) {
    tempIngredIdVal = document.getElementById("ingredientsVId" + varIngredNumb).value;
    tempIngredAmount = document.getElementById("ingredientsVAmountId" + varIngredNumb).value;
    tempIngredUoMId = document.getElementById("ingredientsVUOMId" + varIngredNumb).value;
    tempIngredComId = document.getElementById("ingredientsVCommentId" + varIngredNumb).value;
    var outputString = tempIngredIdVal + "|" + tempIngredAmount + "|" + tempIngredUoMId + "|" + tempIngredComId;
    return outputString;
}


// get compare Instruction row
function processInstructionRow(InstructNumb) {
    tempInstructIdVal = document.getElementById("instructionVId" + varIngredNumb).value;
    tempInstructTempId = document.getElementById("instructionVTempId" + varIngredNumb).value;
    tempInstructUoMId = document.getElementById("instructionVTempId" + varIngredNumb).value;
    var outputString = tempInstructIdVal + "|" + tempInstructTempId + "|" + tempInstructUoMId;
    return outputString;
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
    var outputDiv = document.getElementById('recipeFeedbackDivId');
    outputDiv.classList.remove('hide');
    outputDiv.classList.add('show');
    outputDiv.style.height = '40px';
    outputDiv.style.marginTop = '10px';
    outputDiv.style.color = '#ff0000';
    outputDiv.style.fontSize = '25px';
    var outputPara = document.getElementById('recipeFeedbackParaId');
    outputPara.classList.remove('hide');
    outputPara.classList.add('show');
    outputPara.style.visibility = "visible";
    outputPara.innerHTML = "Please Compvare All Required Fields";
    var mainHeader = document.getElementById('mainHeader');
    mainHeader.scrollIntoView();
    mainHeader.style.marginTop = '145px';
}


// check whether required Recipe fields have been populated
function checkEditRecipeFields() {
    var fieldIds = ['recipeName', 'ingredientsVId1', 'ingredientsVUOMId1', 'instructionVId1'];
    var populatedFields = 0;

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
    // servingsDiv
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

    if (document.getElementById('ingredientsVAmountId1').value < 1) {
        populatedFields++;
        var amount = document.getElementById('ingredientsVAmountId1');
        processClasses(amount);
    }

    fieldIds.forEach(function (item) {
        populatedFields += checkEachRecipeField(item);
    });

    return populatedFields;
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
            boxClass: 'wow',
            animateClass: 'animated',
            offset: 0,
            mobile: true,
            live: true
        });
    wow.init();
});



