firstLoad = false;

$(document).ready(function () {

    $('#recipesLoading').addClass("backg overlay lds-dual-ring");

    // generate table
    let recipeContainer = document.getElementById('recipesContainer');
    recipeTable = document.createElement("table");
    recipeTable.setAttribute("id", "recipeTable");
    recipeTable.classList.add("display","table","table-striped","table-bordered,","table-hover");
    recipeTable.style.width = "100%";
    recipeContainer.append(recipeTable);

    // add header
    tableHeader = recipeTable.createTHead();
    recipeTable.append(tableHeader);
    tableRow = tableHeader.insertRow(0);
    var headers = ["Name", "Photo", "Method", "Time", "Servings", "Ingredients", "Category",
        "Views", "Likes"];
    headers.forEach(addHeader);

    // add body
    tableBody = recipeTable.createTBody();
    recipeTable.append(tableBody);

    // get data
    let options = {};
    options.url = "/List/GetAllRecipes";
    options.type = "POST";
    options.data = {};
    options.dataType = "json";
    options.async = true;
    options.success = function (data) {

        for (let h = 0; h < data.length; h++) {
            tableRow = tableBody.insertRow(h);

            var cell0 = tableRow.insertCell(0);
            var cellValue = data[h].name +
                "<br/><button class=\"btn btn-success tdButtonClass\" onclick=\"viewRecipe(\'" + data[h].ident + "\')\"" +
                " style =\"display:inline\" id=\"tdButton\";>View</button>";         
            cell0.innerHTML = cellValue;

            var cell1 = tableRow.insertCell(1);
            var img = document.createElement("img");
            img.src = data[h].photo;
            img.classList.add("thumbnail", "rounded", "recipeImageThumb");
            //img.style.border = "solid, 0.20em";
            //img.style.borderColor = "#007849";
            cell1.onclick = function() { viewRecipe(data[h].ident);   };
            cell1.append(img);

            var cell2 = tableRow.insertCell(2); cell2.innerHTML = data[h].method;
            var cell3 = tableRow.insertCell(3); cell3.innerHTML = data[h].time;
            var cell4 = tableRow.insertCell(4); cell4.innerHTML = data[h].servings;
            var cell5 = tableRow.insertCell(5); cell5.innerHTML = data[h].ingredients;

            var derivedEqualsOther = data[h].derived.trim().localeCompare("Other");
            var categoryEqualsDerived = data[h].category.trim().localeCompare(data[h].derived.trim());            
            if ((derivedEqualsOther == 0) || (categoryEqualsDerived == 0)) {
                var cell6 = tableRow.insertCell(6);
                cell6.innerHTML = data[h].category;
            } else {
                var cell6b = tableRow.insertCell(6);
                cell6b.innerHTML = data[h].derived + "<br /> (" + data[h].category + ")";
            }

            var cell7 = tableRow.insertCell(7); cell7.innerHTML = data[h].views;
            var cell8 = tableRow.insertCell(8); cell8.innerHTML = data[h].likes;
        }
    
        if(firstLoad == false){
            addDataTable();
            firstLoad = true;
        }
        else{
            $('#recipeTable').DataTable();
        }
        $('#recipesLoading').removeClass("backg overlay lds-dual-ring");
    };

    $.ajax(options);
});


function addHeader(item, index) {
    let cell = tableRow.insertCell(index);
    cell.classList.add("tableHeader");
    cell.innerHTML = "<b>" + item + "</b>";
}


function addDataTable() {
    $('#recipeTable').DataTable({
        paging: true,
        sort: true,
        bLengthChange: true,
        lengthMenu: [[5, 10, 15, -1], [5, 10, 15, "All"]],
        bFilter: true,
        bSort: true,
        bPaginate: true,
        "columnDefs":
        [{
                "targets": 1,
                "orderable": false
        }],
    });
}

function viewRecipe(ident) {
    $('#recipesLoading').addClass("backg overlay lds-dual-ring");
    $.ajax({
        url: "/View/Recipe?ident=" + ident,
        type: "POST",
        async: true
    }).done(function (data) {
        $("#viewTarget").html(data);
        $('#recipesLoading').removeClass("backg overlay lds-dual-ring");
    });
}


