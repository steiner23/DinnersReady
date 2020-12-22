using System.IO;
using Microsoft.AspNetCore.Http;
using DinnersReadyCore.Models.Ingredients;
using DinnersReadyCore.Models.RecipeInstructions;
using DinnersReadyCore.Models.RecipeIngredients;
using DinnersReadyCore.Models.RecipeImages;
using DinnersReadyCore.Models.UnitsOfMeasures;
using System;
using System.Collections.Generic;
using System.Linq;
using DinnersReadyCore.Models.FoodCategories;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;


namespace DinnersReadyCore.Models.Recipes
{

    public class RecipeRepository : IRecipeRepository
    {

        private readonly RecipeContext _recipeContext;

        public RecipeRepository(RecipeContext recipeContext)
        {
            _recipeContext = recipeContext;
        }


        //*********************************************************************
        //Get Update Lists
        //*********************************************************************


        // - user input
        // - 'OrderBy' criteria
        // - Ascending or Descending order
        // - the number of items to get

        public List<string> GetRecipesLike(string userInput, int numberOfRecipes)
        {
            IQueryable<Recipe> allRecipes = _recipeContext.Recipes;

            List<string> allRecipeNames = new List<string>();
            List<string> matchingNames = new List<string>();

            foreach (var rec in allRecipes)
            {
                allRecipeNames.Add(rec.Name);
            }

            foreach (var rName in allRecipeNames)
            {
                if (rName.Contains(userInput))
                {
                    matchingNames.Add(rName);
                }
            }
            return matchingNames;
        }


        //*********************************************************************


        public List<string> GetIngredientsLike(string userInput, int numberOfIngredients)
        {
            List<string> matchingNames = new List<string>();
            if (!string.IsNullOrEmpty(userInput))
            {
                IQueryable<Ingredient> allIngredients = _recipeContext.Ingredients;
                List<string> allIngredientNames = new List<string>();

                foreach (var ing in allIngredients)
                {
                    allIngredientNames.Add(ing.NameSingular);
                }

                /*
                foreach (var ing in allIngredients)
                {
                    allIngredientNames.Add(ing.NamePlural);
                }
                */

                HashSet<string> ingredientSet = new HashSet<string>(allIngredientNames);
                allIngredientNames = ingredientSet.ToList();
                allIngredientNames = allIngredientNames.OrderBy(i => i).ToList();

                foreach (var gName in allIngredientNames)
                {
                    if (gName.ToLower().Contains(userInput.ToLower()))
                    {
                        matchingNames.Add(gName);
                    }
                }
            }
            return matchingNames;
        }


        //*********************************************************************
        // Get Recipe Methods
        //*********************************************************************


        public List<RecipeWithImageIngredients> GetAllTheRecipes()
        {
            List<RecipeWithImageIngredients> rwiiList = new List<RecipeWithImageIngredients>();

            // Get Recipes
            //  IEnumerable<Recipe> recipes = _recipeContext.Recipes.OrderBy(r => "Name").Take(100);
            IEnumerable<Recipe> recipes = _recipeContext.Recipes.OrderBy(r => "Name");
            
            // Get ingredients
            var ingredNameList = _recipeContext.Ingredients
                .Select(i => new IngredientSmall
                { IngredientId = i.IngredientId, NameSingular = i.NameSingular }).ToList();

            foreach (var recep in recipes)
            {        
                
                RecipeWithImageIngredients rwii = new RecipeWithImageIngredients();

                rwii.Name = recep.Name;
                rwii.Method = recep.CookMethod;
                rwii.Servings = recep.Servings.ToString();

                string[] categories = processFoodCategories(recep.FoodCategoryId);
                rwii.Category = categories[0];
                rwii.Derived = categories[1];

                rwii.Views = recep.ViewCount.ToString();
                rwii.Likes = recep.UserLikes.ToString();
                rwii.Ident = recep.RecipeIdent;

                switch (recep.CookingTime) {

                    case "minsLessThan30":
                        rwii.Time = "Under 30 mins";
                        break;

                    case "mins30to59":
                        rwii.Time = "30 to 59 mins";
                        break;

                    case "mins60to90":
                        rwii.Time = "60 to 90 mins";
                        break;

                    case "mins90to120":
                        rwii.Time = "90 to 120 mins";
                        break;

                    case "minsover120":
                        rwii.Time = "Over 120 mins";
                        break;
                }

                // get Recipe Ingredient fields
                List<string> ingredNames = new List<string>();

                var ingredIdList = _recipeContext.RecipeIngredients.Where(r => r.RecipeId == recep.RecipeId)
                    .Select(i => i.IngredientId).ToList();

                foreach(var ingredId in ingredIdList) {
                    foreach(var ingredName in ingredNameList) {
                        if(ingredId == ingredName.IngredientId)
                        {
                            ingredNames.Add(ingredName.NameSingular);
                            break;
                        }
                    }
                }

                HashSet<string> namesSet = new HashSet<string>(ingredNames);
                ingredNames = namesSet.ToList();

                string queryString = "";
                foreach(var q in ingredNames) {
                    queryString = queryString + q + ", ";
                }

                if(queryString.Length > 0)
                {
                    queryString = queryString.Remove(queryString.LastIndexOf(","), 1);
                }

                rwii.Ingredients = queryString;

                // get Recipe Image fields
                RecipeImage recepImage = _recipeContext.RecipeImages.FirstOrDefault(i => i.RecipeIdent == recep.RecipeIdent);
                if (recepImage != null) {
                    string imageBase64Data = Convert.ToBase64String(recepImage.Image);
                    string imageDataURL = string.Format("data:" + recepImage.ContentType + ";base64,{0}", imageBase64Data);
                    rwii.Photo = imageDataURL;
                }

                rwiiList.Add(rwii);
            }
            return rwiiList;
        }


        //*********************************************************************


        //Get Recipes selecting:
        // - 'OrderBy' criteria
        // - Ascending or Descending order
        // - the number of Recipes to get

        public IEnumerable<Recipe> GetRecipes(string orderBy, bool ascending, int numberOfRecipes)
        {
            IQueryable<Recipe> recipes;
            var orderInfo = typeof(Recipe).GetProperty(orderBy);

            if (ascending == false)
            {
                if (numberOfRecipes == 0)
                {
                    recipes = _recipeContext.Recipes.OrderByDescending(r => orderInfo);
                }
                else
                {
                    recipes = _recipeContext.Recipes.OrderByDescending(r => orderInfo).Take(numberOfRecipes);
                }
            }
            else
            {
                if (numberOfRecipes == 0) // if 0 then pass all Recipes
                {
                    recipes = _recipeContext.Recipes.OrderBy(r => orderInfo);
                }
                else
                {
                    recipes = _recipeContext.Recipes.OrderBy(r => orderInfo).Take(numberOfRecipes);
                }
            }
            return recipes.AsEnumerable();
        }


        //*********************************************************************


        // Get a Recipe (high-level) by Recipe Name
        public IQueryable<Recipe> GetARecipe(string recipeName)
        {
            IQueryable<Recipe> recipe = _recipeContext.Recipes.Where(r => r.Name == recipeName);
            return recipe;
        }


        //*********************************************************************


        // Get a Recipe (high-level) by Recipe Id
        public IQueryable<Recipe> GetARecipe(int recipeId)
        {
            IQueryable<Recipe> recipe = _recipeContext.Recipes.Where(r => r.RecipeId == recipeId);
            return recipe;
        }


        //*********************************************************************


        // Get a Recipe Id (high-level) by GenData timestamp
        public int GetARecipeByGenData(string genDate)
        {
            DateTime genDateTime = Convert.ToDateTime(genDate);
            Recipe recipe = (Recipe)_recipeContext.Recipes.Where(r => r.GenDate == genDateTime);
            return recipe.RecipeId;
        }


        //*********************************************************************


        //Get a Recipe (with details) by Recipe Ident
        public RecipeWithDetails GetRecipeWithDetails(string recipeIdent)
        {
            RecipeWithDetails recipeWithDetails = new RecipeWithDetails();

            IQueryable<Recipe> recipe = _recipeContext.Recipes.Where(r => r.RecipeIdent == recipeIdent);
            Recipe recep = recipe.SingleOrDefault();

            recipeWithDetails.Name = recep.Name;
            recipeWithDetails.Description = recep.Description;
            recipeWithDetails.Course = recep.Course;
            recipeWithDetails.CookMethod = recep.CookMethod;
            recipeWithDetails.Servings = recep.Servings;
            recipeWithDetails.CookingTime = recep.CookingTime;
            recipeWithDetails.RecipeIdent = recep.RecipeIdent;
            recipeWithDetails.AdditionalNotes = recep.AdditionalNotes;
            recipeWithDetails.GoesWith = recep.GoesWith;
            recipeWithDetails.ViewCount = recep.ViewCount;
            recipeWithDetails.UserLikes = recep.UserLikes;
            recipeWithDetails.Season = recep.Season;
            recipeWithDetails.Cuisine = recep.Cuisine;
            recipeWithDetails.Event = recep.Event;
            recipeWithDetails.DietaryRequirement = recep.DietaryRequirement;
            recipeWithDetails.Freezable = recep.Freezable;
            recipeWithDetails.ReviewedStatus = recep.ReviewedStatus;
            recipeWithDetails.FoodCategory = processFoodCategories(recep.FoodCategoryId)[0];
            recipeWithDetails.DerivedCategory = processFoodCategories(recep.FoodCategoryId)[1];

            List<string> IngredientNames = new List<string>();
            List<double> IngredientBaseAmounts = new List<double>();
            List<double> IngredientViewAmounts = new List<double>();
            List<string> OriginalUnitOfMeasures = new List<string>();
            List<string> IngredientComments = new List<string>();
            List<string> IngredientIdents = new List<string>();

            //get recipe ingredients, then loop through the ingredients to save information
            IQueryable<RecipeIngredient> recipeIngredients = _recipeContext.RecipeIngredients.Where(r => r.RecipeId == recep.RecipeId);
            foreach (var ringredient in recipeIngredients)
            {
                RecipeIngredient recepIngred = ringredient;
                Ingredient ingred = _recipeContext.Ingredients.Where(i => i.IngredientId == recepIngred.IngredientId).SingleOrDefault();

                IngredientNames.Add(ingred.NameSingular);
                OriginalUnitOfMeasures.Add(ringredient.OriginalUnitOfMeasure);
                IngredientBaseAmounts.Add(ringredient.Amount);
                IngredientIdents.Add(ringredient.RecipeIngredientIdent);

                //from the original unit of measure, identify the base unit of measure
                //in which the amount value has been stored
                //then multiply by the conversion value to get
                //the display amount in the display unit of measure
                double displayAmount = ConvertFromBaseToOrigUoM(ringredient.Amount, ringredient.OriginalUnitOfMeasure);
                IngredientViewAmounts.Add(displayAmount);

                IngredientComments.Add(recepIngred.Comment);
            }

            recipeWithDetails.IngredientNames = IngredientNames;
            recipeWithDetails.IngredientBaseAmounts = IngredientBaseAmounts;
            recipeWithDetails.IngredientViewAmounts = IngredientViewAmounts;
            recipeWithDetails.OriginalUnitOfMeasures = OriginalUnitOfMeasures;
            recipeWithDetails.IngredientComments = IngredientComments;
            recipeWithDetails.RecipeIngredientIdents = IngredientIdents;

            List<string> InstructionDescriptions = new List<string>();
            List<double> TemperatureBaseValues = new List<double>();
            List<double> TemperatureValues = new List<double>();
            List<string> OriginalTemperatureUnitOfMeasure = new List<string>();
            List<string> InstructionIdents = new List<string>();

            //get recipe instructions, then loop through the instructions, order by steps, to save information
            IQueryable<RecipeInstruction> instructions = _recipeContext.RecipeInstructions.Where(n => n.RecipeId == recep.RecipeId).OrderBy(m => m.InstructionStepId);
            foreach (var rinstruction in instructions)
            {
                RecipeInstruction instruct = rinstruction;
                InstructionDescriptions.Add(instruct.Description.ToString());
                OriginalTemperatureUnitOfMeasure.Add(instruct.OriginalTemperatureUnitOfMeasure.ToString());
                TemperatureBaseValues.Add(instruct.TemperatureValue);
                InstructionIdents.Add(instruct.RecipeInstructionIdent);

                //from the original unit of measure, identify the base unit of measure
                //in which the temperature has been stored
                //then multiply by the conversion value to get
                //the display temperature in the display unit of measure

                double displayTemperature = 0.0;
                if (instruct.TemperatureValue > 0)
                {
                    displayTemperature = ConvertFromBaseToOrigUoM(instruct.TemperatureValue, instruct.OriginalTemperatureUnitOfMeasure);                    
                }
                TemperatureValues.Add(displayTemperature);
            }

            recipeWithDetails.InstructionDescriptions = InstructionDescriptions;
            recipeWithDetails.TemperatureBaseValues = TemperatureBaseValues;
            recipeWithDetails.TemperatureValues = TemperatureValues;
            recipeWithDetails.OriginalTemperatureUnitOfMeasure = OriginalTemperatureUnitOfMeasure;
            recipeWithDetails.RecipeInstructionIdents = InstructionIdents;


            // get Recipe Image fields
            RecipeImage recepImage = _recipeContext.RecipeImages.FirstOrDefault(i => i.RecipeIdent == recep.RecipeIdent);
            if (recepImage != null)
            {
                string imageBase64Data = Convert.ToBase64String(recepImage.Image);
                string imageDataURL = string.Format("data:" + recepImage.ContentType + ";base64,{0}", imageBase64Data);
                recipeWithDetails.Photo = imageDataURL;
            }

            // increment the Views for the Recipe
            // await async IncrementRecipeViews(recep.RecipeId);
            recep.ViewCount++;

            _recipeContext.SaveChanges();

            return recipeWithDetails;
        }


        //*********************************************************************


        public List<string> GetMainFoodCategories(){
            
            List<string> mainFoodCategories = new List<string>();
            try
            {                
                IEnumerable<FoodCategory> foodCategories = from fc in _recipeContext.FoodCategories
                    .OrderBy(x => x.FoodCategoryName)
                    select fc;
                    
                HashSet<string> foodCategoriesSet = new HashSet<string>();

                foreach(FoodCategory foodcat in foodCategories){
                    foodCategoriesSet.Add(foodcat.FoodCategoryName.ToString());
                }

                mainFoodCategories = foodCategoriesSet.ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return (mainFoodCategories);         
        }


        //*********************************************************************


        public void IncrementRecipeViews(int recipeId)
        {
            Recipe recipe = _recipeContext.Recipes.FirstOrDefault(r => r.RecipeId == recipeId);
            recipe.ViewCount++;
            _recipeContext.SaveChangesAsync();
        }


        //*********************************************************************
        //Add Recipe Method
        //*********************************************************************


        public string AddRecipe(string recipeName, string description,
            string course, string cookingMethod, string servings, string cookingTime, 
            string additionalNotes, string goesWithNotes, string season, string cuisine, string anevent,
            string dietaryRequirement, bool freezable, string mainFoodCategory,
            List<string>  ingredientsName, List<string> amountsName, List<string> uomName,
            List<string> commentsName, List<string> instructionsName, List<string> instructionTempName,
            List<string> originalTemperatureUoMName, string recipeIdent)
        {

            DateTime timestamp = DateTime.UtcNow;

            if(recipeIdent.Equals("none")) {
                recipeIdent = GenNewRecipeIdent("Recipe");
            }

            // get FoodCategoryId from MainFoodCategoryName
            var foodCategoryId = getFoodCategoryId(mainFoodCategory);

            Recipe newRecipe = new Recipe
            {
                Name = recipeName,
                Description = description,
                Course = course,
                CookMethod = cookingMethod,
                Servings = int.Parse(servings),
                CookingTime = cookingTime,
                AdditionalNotes = additionalNotes,
                GoesWith = goesWithNotes,
                ViewCount = 0,
                UserLikes = 0,
                Season = season,
                Cuisine = cuisine,
                Event = anevent,
                DietaryRequirement = dietaryRequirement,
                Freezable = freezable,
                ReviewedStatus = null,
                FoodCategoryId = foodCategoryId,
                GenDate = timestamp,
                RecipeIdent = recipeIdent
            };

            _recipeContext.Add(newRecipe);
            _recipeContext.SaveChanges();

            int RecipeId = newRecipe.RecipeId;

            //generate new Recipe Ingredients using the new Recipe Id
            for (int i = 0; i < ingredientsName.Count; i++)
            {
                var commentvalue = "";
                if ((!String.IsNullOrEmpty(commentsName[i])) && (commentsName[i] != "--optional--")) {
                    commentvalue = commentsName[i];
                } else {
                    commentvalue = "--none--";
                }

                recipeIdent = GenNewRecipeIdent("RecipeIngredient");

                _recipeContext.RecipeIngredients.Add(
                    new RecipeIngredient
                    {
                        RecipeId = RecipeId,
                        IngredientId = GetIngredientIdFromIngredientName(ingredientsName[i]),
                        OriginalUnitOfMeasure = uomName[i],
                        Amount = ConvertFromOrigUoMToBase(double.Parse(amountsName[i]), uomName[i]),
                        Comment = commentvalue,
                        RecipeIngredientIdent = recipeIdent
                    }
                ); ;
            }
            _recipeContext.SaveChanges();

            //generate new Recipe Instructions using the new Recipe Id
            for (int i = 0; i < instructionsName.Count; i++)
            {
                var tempUoM = "--none--";
                var tempval = 0.0;
                var descrip = instructionsName[i];
                if ((!String.IsNullOrEmpty(originalTemperatureUoMName[i])) && (originalTemperatureUoMName[i] != "n/a"))
                {
                    tempUoM = originalTemperatureUoMName[i];
                    tempval = ConvertFromOrigUoMToBase( double.Parse(instructionTempName[i]), originalTemperatureUoMName[i] );
                }

                recipeIdent = GenNewRecipeIdent("RecipeInstruction");

                _recipeContext.RecipeInstructions.Add(

                    new RecipeInstruction
                    {
                        RecipeId = RecipeId,
                        InstructionStepId = (i),
                        Description = descrip,
                        OriginalTemperatureUnitOfMeasure = tempUoM,
                        TemperatureValue = tempval,
                        RecipeInstructionIdent = recipeIdent
                    }
                );
            }
            _recipeContext.SaveChanges();

            var recipeAdded = timestamp.ToString();
            return (recipeAdded);
        }


        public string ProcessImage(IFormFile recipeimage){

            string RecpIdent = GenNewRecipeIdent("Recipe");
            Byte[] fileBytes;

            using (var memoryStream = new MemoryStream()){
                recipeimage.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            _recipeContext.RecipeImages.Add(

                new RecipeImage
                {
                    ImageName = recipeimage.FileName,
                    ContentType = recipeimage.ContentType,
                    Image = fileBytes,
                    RecipeIdent = RecpIdent
                }
            );
            // }

            _recipeContext.SaveChangesAsync();
            return RecpIdent;     
        }


        //*********************************************************************
        // Update Recipe Method
        //*********************************************************************

        public bool ProcessTheUpdatedImage(IFormFile recipeImage, string RecipeIdent) {

            bool updateSuccess = false;
            RecipeImage localRecepImage = _recipeContext.RecipeImages.FirstOrDefault(r => r.RecipeIdent == RecipeIdent);
            Byte[] fileBytes;

            using (var memoryStream = new MemoryStream()){
                recipeImage.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }            

            if(localRecepImage != null) 
            {
                localRecepImage.ImageName = recipeImage.FileName;
                localRecepImage.ContentType = recipeImage.ContentType;
                localRecepImage.Image = fileBytes;         

                _recipeContext.RecipeImages.Update(localRecepImage);
                var success = _recipeContext.SaveChanges();
             //   if(success.Result > 0) 
                { updateSuccess = true; }
            }
            else
            {
                RecipeImage newRecipeImage = new RecipeImage 
                {
                    ImageName = recipeImage.FileName,
                    ContentType = recipeImage.ContentType,
                    Image = fileBytes,
                    RecipeIdent = RecipeIdent
                };

                _recipeContext.Add(newRecipeImage);
                var success = _recipeContext.SaveChanges();
                if(success == 0) { updateSuccess = false; }
            }            
            return updateSuccess;
        }


        public bool UpdateStandardFields(string[][] updates, string RecipeIdent) {

            bool updateSuccess = false;
            Recipe recep = _recipeContext.Recipes.FirstOrDefault(r => r.RecipeIdent == RecipeIdent);

            for(int u = 0; u < updates.Length; u++) {

                switch(updates[u][0]){ 

                    case "recipeName" :
                        recep.Name = updates[u][1];
                        break;

                    case "servings" :
                        recep.Servings = Int32.Parse(updates[u][1]);
                        break;    

                    case "course" :
                        recep.Course = updates[u][1];
                        break;

                    case "description" :
                        recep.Description = updates[u][1];
                        break;

                    case "cookingMethod" :
                        recep.CookMethod = updates[u][1];
                        break;

                    case "cookingTime" :
                        recep.CookingTime = updates[u][1];
                        break;

                    case "mainFoodCategory" :
                        recep.FoodCategoryId = GetFoodCategoryId(updates[u][1]);
                        break;

                    case "season" :
                        recep.Season = updates[u][1];
                        break;

                    case "cuisine" :
                        recep.Cuisine = updates[u][1];
                        break;

                    case "freeze" :
                        recep.Freezable = Boolean.Parse(updates[u][1]);
                        break;                      

                    case "anevent" :
                        recep.Event = updates[u][1];
                        break; 

                    case "dietaryRequirement" :
                        recep.DietaryRequirement = updates[u][1];
                        break;

                    case "additionalNotes" :
                        recep.AdditionalNotes = updates[u][1];
                        break;                        

                    case "goesWithNotes" :
                        recep.GoesWith = updates[u][1];
                        break;  
                } // end switch
            } // end for

            _recipeContext.Recipes.Update(recep);
            var success = _recipeContext.SaveChangesAsync();
            if(success.Result > 0)
            { updateSuccess = true; }        
            
            return updateSuccess;
        }


        public bool UpdateAddNewFields(string[][] added, string RecipeIdent) {
    
            bool updateSuccess = false;    
            Recipe recep = _recipeContext.Recipes.FirstOrDefault(r => r.RecipeIdent == RecipeIdent);

            for(var i = 0; i < added.Length; i++)
            {
                if(added[i][0].ToString() == "Ingredient"){

                    RecipeIngredient newRecipeIngredient = new RecipeIngredient
                    {
                        RecipeId = recep.RecipeId,
                        IngredientId = GetIngredientIdFromIngredientName(added[i][1]),
                        Amount = Double.Parse(added[i][2]),
                        OriginalUnitOfMeasure = added[i][3], 
                        Comment = added[i][4],
                        RecipeIngredientIdent = GenNewRecipeIdent("RecipeIngredient")
                    };

                    _recipeContext.Add(newRecipeIngredient);
                    var success = _recipeContext.SaveChanges();
                    if(success == 0) { updateSuccess = false; }          
                } 
                else{
                    
                    List<RecipeInstruction> recepInstruct = _recipeContext.RecipeInstructions.Where(i => i.RecipeId == recep.RecipeId).ToList();
                    int stepValue = 0;
                    if(recepInstruct.Count() > 0) {
                        var temp = 0;
                        for(int c = 0; c < recepInstruct.Count(); c++) {
                            if(recepInstruct[c].InstructionStepId > temp) {
                                temp = recepInstruct[c].InstructionStepId;
                            }
                        }
                        stepValue = temp;
                    }
                    stepValue++;

                    RecipeInstruction newRecipeInstruction = new RecipeInstruction
                    {
                        RecipeId = recep.RecipeId,                       
                        InstructionStepId = stepValue,
                        Description = added[i][1],
                        TemperatureValue = Double.Parse(added[i][2]),
                        OriginalTemperatureUnitOfMeasure = added[i][3],
                        RecipeInstructionIdent = GenNewRecipeIdent("RecipeInstruction")
                    };

                    _recipeContext.Add(newRecipeInstruction);
                    var success = _recipeContext.SaveChanges();
                    if(success == 0) { updateSuccess = false; }
                } // end if-else
            } // end for
            return updateSuccess;
        } // end method


        public bool RemoveDeletedfields(string[][] updates, string RecipeIdent) {

            bool updateSuccess = false;
            for(int r = 0; r < updates.Length; r++) {
                
                if(updates[r][0] == "Ingredient") {
                    var recepIngredIdent = updates[r][1];
                    RecipeIngredient recepIngred = _recipeContext.RecipeIngredients.FirstOrDefault(i => i.RecipeIngredientIdent == recepIngredIdent);
                    _recipeContext.Remove(recepIngred);
                    var success = _recipeContext.SaveChanges();
                    if(success == 0) { updateSuccess = false; }
                }
                else 
                {
                    var recepInstructIdent = updates[r][1];
                    RecipeInstruction recepInstruct = _recipeContext.RecipeInstructions.FirstOrDefault(s => s.RecipeInstructionIdent == recepInstructIdent);
                    _recipeContext.Remove(recepInstruct);
                    var success = _recipeContext.SaveChanges();
                    if(success == 0) { updateSuccess = false; }
                }
            } // end for
        return updateSuccess;
        } // end method


        public bool UpdateImage(IFormFile recipeimage, string RecipeIdent)
        {
            var updated = true;
            RecipeImage recipeImage = _recipeContext.RecipeImages.FirstOrDefault(i => i.RecipeIdent == RecipeIdent);
            Byte[] fileBytes;

            using (var memoryStream = new MemoryStream())
            {
                recipeimage.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            recipeImage.ImageName = recipeimage.FileName;
            recipeImage.ContentType = recipeimage.ContentType;
            recipeImage.Image = fileBytes;
            recipeImage.RecipeIdent = RecipeIdent;             

            _recipeContext.SaveChangesAsync();
            return updated;
        }
        

        public string GenNewRecipeIdent(string Table)
        {
            int stringLength = 15;
            string valString = GetUniqueKey(stringLength);

            switch(Table){

                case "Recipe": 
                    string temp = valString + "r";
                    break;

                case "RecipeIngredient":
                    valString = valString + "g";
                    break;

                case "RecipeInstruction":
                    valString = valString + "i";
                    break;                
            }

            var recpIdent = "";

            // check Ident string doesn't alreadt exists
            switch (Table) {

                case "Recipe":
                    recpIdent = (from r in _recipeContext.Recipes
                                where r.RecipeIdent == valString
                                select r.RecipeIdent.ToString()).ToString();
                break;

                case "RecipeIngredient":
                    recpIdent = (from r in _recipeContext.RecipeIngredients
                                    where r.RecipeIngredientIdent == valString
                                    select r.RecipeIngredientIdent.ToString()).ToString();
                    break;

                case "RecipeInstruction":
                    recpIdent = (from r in _recipeContext.RecipeInstructions
                                    where r.RecipeInstructionIdent == valString
                                    select r.RecipeInstructionIdent.ToString()).ToString();
                    break;
            }


            if (recpIdent.Any())
            {
                valString = GetUniqueKey(stringLength);
            }
            return (valString);
        }

        public string GetUniqueKey(int size)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();

            byte[] data = new byte[4 * size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }
            return (result.ToString());
        }


        public int getFoodCategoryId(string mainFoodCategory) 
        {
            FoodCategory foodCategory = _recipeContext.FoodCategories.FirstOrDefault(f => f.FoodCategoryName == mainFoodCategory);
            return foodCategory.FoodCategoryId;
        }


        //*********************************************************************


        // Get IngredientId from an Ingredient Name
        public int GetIngredientIdFromIngredientName(string ingredientName)
        {
            var ingred = _recipeContext.Ingredients.FirstOrDefault(i => i.NameSingular.Equals(ingredientName));
            if(ingred != null)
            {
                return(ingred.IngredientId);
            }
            else{
                ingred = _recipeContext.Ingredients.FirstOrDefault(i => i.NamePlural.Equals(ingredientName));
                return(ingred.IngredientId);                   
            }
        }


        //Get FoodCategory Name and Derived FoodCategory Name from a Food Category Id
        public string[] processFoodCategories(int localFoodCategoryId)
        {
            FoodCategory foodCategory = _recipeContext.FoodCategories.FirstOrDefault(f => f.FoodCategoryId == localFoodCategoryId);
            string[] categories = { foodCategory.FoodCategoryName, foodCategory.DerivedFoodCategoryName };
            return categories;
        }


        // Get FoodCategory Id from a Food Category name
        public int GetFoodCategoryId(string foodCategoryName)
        {
            var foodCategory = _recipeContext.FoodCategories.FirstOrDefault(f => f.FoodCategoryName.Equals(foodCategoryName));
            if (foodCategory != null)
            {
                return foodCategory.FoodCategoryId;
            }
            else { return 0; }
        }


        //*********************************************************************
        // Data Validation Methods
        //*********************************************************************


        // Validate the new Recipe before adding
        public string ProcessRecipe(string recipeName, string description, 
            string course, string cookingMethod, string servings, string cookingTime, 
            string additionalNotes, string goesWithNotes, string season, string cuisine, string anevent,
            string dietaryRequirement, bool freezable, string mainFoodCategory,
            List<string>  ingredientsName, List<string> amountsName, List<string> uomName, 
            List<string> commentsName, List<string> instructionsName, List<string> instructionTempName,
            List<string> originalTemperatureUoMName, string recipeIdent)
        {

            var validationSuccess = true;
            var genDate = "";

            // Name
            if (String.IsNullOrEmpty(recipeName)) { validationSuccess = false; }
            else { if (CheckIfRecipeNameAlreadyExists(recipeName)) { validationSuccess = false; } }

            // Course
            var mealMatch = false;
            string[] courses = { "Breakfast", "Lunch", "Dinner", "Other" };
            string[] meals = course.Split("|");
            foreach(var item in courses) {
                foreach(var meal in meals) {
                    if (String.Compare(item, meal) == 0)
                    {
                        mealMatch = true;
                        break;
                    }
                }
            }
            if(mealMatch == false) { 
                validationSuccess = false; 
            }

            // Cooking Method
            string[] cookingMethods = { "Oven", "Hob", "Slowcooker", "Other" };
            if (Array.Exists(cookingMethods, element => element == cookingMethod) == false) {
                validationSuccess = false;
            }

            // Servings
            Double serving = Convert.ToDouble(servings);
            if((serving < 1) || (serving > 100)) {
                validationSuccess = false;
            }

            // Cooking Time
            string[] cookingTimes = { "minsLessThan30", "mins30to59", "mins60to90", "mins90to120", "minsover120" };
            if(Array.Exists(cookingTimes, element => element == cookingTime) == false) {
                validationSuccess = false;
            }

            // Freezable
            if((freezable != true) && (freezable != false)) {
                validationSuccess = false;
            }
            
            if (!String.IsNullOrEmpty(season)) {
                if (CheckSeasonValid(season) == false) { validationSuccess = false; }
            }

            if (!String.IsNullOrEmpty(cuisine)) {
                if (CheckCuisineValid(cuisine) == false) { validationSuccess = false; }
            }

            if (!String.IsNullOrEmpty(anevent)) {                
                if (CheckEventValid(anevent) == false) { validationSuccess = false; }
            }

            if (!String.IsNullOrEmpty(dietaryRequirement)) {
                if (CheckDietaryRequirement(dietaryRequirement) == false) { validationSuccess = false; }
            }
            
            if(CheckMainFoodCategory(mainFoodCategory) == false) {validationSuccess = false;}

            for(var i = 0; i < ingredientsName.Count(); i++)
            {
                if(CheckIngredient(ingredientsName[i]) == false) {validationSuccess = false;}
                // the instruction temperature could be 0 or below if chilling / setting / freezing ingredients
                // if(Convert.ToDouble(amountsName[i]) <= 0) {validationSuccess = false;} 
                if (CheckUnitOfMeasure(uomName[i]) == false)  {validationSuccess = false;}
            }

            for(var t = 0; t < instructionTempName.Count(); t++)
            {
                if((!String.IsNullOrEmpty(instructionTempName[t])) && (!String.IsNullOrEmpty(originalTemperatureUoMName[t])))
                {
                    if(Convert.ToDouble(instructionTempName[t]) < 0) {validationSuccess = false;}
                    if(CheckUnitOfMeasure(originalTemperatureUoMName[t]) == false) {validationSuccess = false;}                     
                }

                if ( (( !String.IsNullOrEmpty(instructionTempName[t] )) && ( String.IsNullOrEmpty(originalTemperatureUoMName[t] ))) ||
                    ((  String.IsNullOrEmpty(instructionTempName[t] )) && ( !String.IsNullOrEmpty(originalTemperatureUoMName[t] ))) )
                    {
                        validationSuccess = false;
                    }
            }

            if(validationSuccess == true)
            {
                genDate = AddRecipe(recipeName, description, course, cookingMethod,
                servings, cookingTime, additionalNotes, goesWithNotes, season, cuisine, anevent,
                dietaryRequirement, freezable, mainFoodCategory, ingredientsName, amountsName, uomName, 
                commentsName, instructionsName, instructionTempName, originalTemperatureUoMName, recipeIdent);
            }

            if(!string.IsNullOrEmpty(genDate)){
                return(genDate);
            }else{
                return "RecipeNotAdded"; 
            }
        }

        //*********************************************************************


        //Check whether recipe name already exists
        public bool CheckIfRecipeNameAlreadyExists(string recipeName)
        {
            Boolean exists = false;
            IEnumerable<Recipe> recipe = new List<Recipe>();

            recipe = from r in _recipeContext.Recipes
                where r.Name.Equals(recipeName)
                select r;

            if(recipe.Count() > 0)
            {
                exists = true;
            }
            return exists;
        }


        //*********************************************************************


        //Check the Season value is valid
        public bool CheckSeasonValid(string season)
        {
            bool exists = false;
            season = season.ToLower();
            string[] fourSeasons = { "spring", "summer", "autumn", "winter", "allyearround" };
            if (fourSeasons.Contains(season))  { exists = true; }
            return exists;
        }


        //*********************************************************************


        //Check the Cuisine value is valid
        public bool CheckCuisineValid(string cuisine)
        {
            bool exists = false;
            cuisine = cuisine.ToLower();
            string[] cuisines = { "belgian", "british", "chinese", "dutch", "french", "indian", "irish", "italian", "spanish", "thai", "other" };
            if (cuisines.Contains(cuisine)) { exists = true; }
            return exists;
        }


        //*********************************************************************


        //Check the Event value is valid
        public bool CheckEventValid(string anEvent)
        {
            bool exists = false;
            anEvent = anEvent.ToLower();
            string[] events = { "christmas", "birthday", "newyear", "easter", "party", "barbeque", "somethingelse" };
            if (events.Any(anEvent.Contains)) { exists = true; }
            return exists;
        }


        //*********************************************************************


        //Check the Dietary Requirement is valid
        public bool CheckDietaryRequirement(string dietaryRequirement)
        {
            bool exists = false;
            dietaryRequirement = dietaryRequirement.ToLower();
            string[] dietaryRequirements = { "kosher", "gluten-free", "halal", "lactose-free", "vegan", "vegetarian", "none" };
            if (dietaryRequirements.Any(dietaryRequirement.Contains)) { exists = true; }
            return exists;
        }


        //*********************************************************************


        //Check the Main Ingredient value is a valid
        public bool CheckMainFoodCategory(string mainIngredient)
        {
            bool exists = false;
            var mainIngred = _recipeContext.FoodCategories.FirstOrDefault(f => f.FoodCategoryName.Equals(mainIngredient));
            if (mainIngred != null)
            {
                exists = true;
            }
            return exists;
        }


        //*********************************************************************


        //Check the Ingredient value is a valid
        public bool CheckIngredient(string ingredient)
        {
            bool exists = false;
            var ingred = _recipeContext.Ingredients.FirstOrDefault(i => i.NameSingular.Equals(ingredient));
            if (ingred != null)
            {
                exists = true;
            }
            else {
                var ingred2 = _recipeContext.Ingredients.FirstOrDefault(i => i.NamePlural.Equals(ingredient));
                if (ingred2 != null)
                {
                    exists = true;
                }
            }
            return exists;
        }


        //*********************************************************************


        //Check the UnitOfMeasure is valid
        public bool CheckUnitOfMeasure(string unitOfMeasure)
        {
            bool exists = false;
            IEnumerable<UnitOfMeasure> Uoms = _recipeContext.UnitsOfMeasures;
            foreach(var Uom in Uoms)
            {
                string temp = Uom.DisplayUomName.Trim();
                if(temp.Equals(unitOfMeasure.Trim()) == true)
                {
                    exists = true;
                    break;
                }
            }
            return exists;
        }


        //*********************************************************************
        //Delete Recipe Methods
        //*********************************************************************


        //Delete Recipe by Recipe Name
        public bool DeleteRecipe(string recipeName)
        {
            var recipeDeleted = false;
            try
            {
                var recipe = _recipeContext.Recipes.FirstOrDefault(r => r.Name.Equals(recipeName));
                if (recipe != null)
                {
                    _recipeContext.Recipes.Remove(recipe);
                    recipeDeleted = true;
                }
                _recipeContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return recipeDeleted;
        }

        //*********************************************************************


        //Delete Recipe by Recipe Id
        public bool DeleteRecipe(int recipeId)
        {
            var recipeDeleted = false;
            try
            {
                var recipe = _recipeContext.Recipes.FirstOrDefault(r => r.RecipeId.Equals(recipeId));
                if (recipe != null)
                {
                    _recipeContext.Recipes.Remove(recipe);
                    recipeDeleted = true;
                }
                _recipeContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return recipeDeleted;
        }


        //*********************************************************************
        //Unit of Measure and Amount Conversion Methods
        //*********************************************************************

        public double ConvertFromBaseToOrigUoM(double SourceAmount, string OrigUnitOfMeasure)
        {
            double displayAmount = 0.0;
            UnitOfMeasure uom = _recipeContext.UnitsOfMeasures.FirstOrDefault(u => u.DisplayUomName.Trim() == OrigUnitOfMeasure.Trim());

            var shortUoMSymbol = uom.UnitOfMeasureSymbol.Replace("\t", " ");
            switch (shortUoMSymbol.Trim())
            {
                case "°F":
                    displayAmount = (((SourceAmount / 5) * 9) + 32);
                    break;

                case "GM":
                    var displayAmountInt = Math.Round((((SourceAmount - 135) / 14) + 1), 0);
                    displayAmount = displayAmountInt;
                    break;

                default:
                    double conversionRate = uom.MultiplyConversionToIngredientRate;
                    displayAmount = (conversionRate * SourceAmount);
                    displayAmount = Math.Round(displayAmount, 2);
                    break;
            }
            return displayAmount;
        }

        //*********************************************************************

        public double ConvertFromOrigUoMToBase(double SourceAmount, string OrigUnitOfMeasure)
        {
            double displayAmount = 0;
            UnitOfMeasure uom = _recipeContext.UnitsOfMeasures.FirstOrDefault(u => u.DisplayUomName.Trim() == OrigUnitOfMeasure);

            var shortUoMSymbol = uom.UnitOfMeasureSymbol.Replace("\t", " ");
            switch (shortUoMSymbol.Trim())
            {
                case "°F":
                    displayAmount = (((SourceAmount - 32) * 5) / 9);
                    break;

                case "GM":
                    displayAmount = (((SourceAmount - 1) * 14) + 135);
                    break;

                default:
                    double conversionRate = uom.MultiplyConversionToBaseRate;
                    displayAmount = (conversionRate * SourceAmount);
                    break;

            }
            return displayAmount;
        }

        //*********************************************************************


        // get the range of Unit of Measure options based on the Type: Dry, Wet, Length, Temperature
        // and (optionaly) by System too: Metric, Imperial, All
        public List<string> GetUnitsOfMeasureRange(string UofMType, string UofMSystem)
        {
            List<string> unitsOfMeasureRange = new List<string>();

            if (UofMSystem == "All")
            {
                IQueryable<UnitOfMeasure> unitsOfMRange = _recipeContext.UnitsOfMeasures.Where(u => u.DryOrWetUnitOfMeasure.Equals(UofMType));
                foreach (var uOfM in unitsOfMRange)
                { unitsOfMeasureRange.Add(uOfM.DisplayUomName); }
            }
            else // if 'Metric' or 'Imperial'
            {
                IQueryable<UnitOfMeasure> unitsOfMRange = _recipeContext.UnitsOfMeasures.Where(u => u.DryOrWetUnitOfMeasure.Equals(UofMType))
                    .Where(u => u.SystemOfMeasureName.Equals(UofMSystem));
                foreach (var uOfM in unitsOfMRange)
                { unitsOfMeasureRange.Add(uOfM.DisplayUomName); }
            }

            return unitsOfMeasureRange;
        }


        //*********************************************************************


        // get a list of Unit of Measure options for the Type (Dry or Wet) for a specific ingredient
        public List<string> GetUnitsOfMeasureSymbolsForIngredientType(string Ingredient)
        {
            List<string> unitsOfMeasureRange = new List<string>();

            Ingredient ingred = _recipeContext.Ingredients.FirstOrDefault(i => i.NameSingular == Ingredient);

            IEnumerable<UnitOfMeasure> unitsOfMRange = _recipeContext.UnitsOfMeasures.ToList();
            List<string> uomDisplayNameList = new List<string>();

            if (ingred != null)
            {
                foreach (var uOMR in unitsOfMRange)
                {
                    var dryOrWetUnitOfMeasureTrim = uOMR.DryOrWetUnitOfMeasure.Trim();
                    if (dryOrWetUnitOfMeasureTrim == ingred.TypeOfMeasureName.Trim())
                    {
                        uomDisplayNameList.Add(uOMR.DisplayUomName);
                    }
                }
            }

            HashSet<string> unitsOfMRangeSet = new HashSet<string>(uomDisplayNameList);
            uomDisplayNameList = unitsOfMRangeSet.ToList();

            return uomDisplayNameList;
        }


        //*********************************************************************


        // get a list of Temperature Unit of Measure options
        public List<string> GetTemperatureUnitsOfMeasures()
        {
            List<string> temperatureStrings = new List<string>();
            var unitsOfMTemperature = from t in _recipeContext.UnitsOfMeasures
                            select t; 

            foreach (var temp in unitsOfMTemperature)
            {
                if (temp.DryOrWetUnitOfMeasure.Trim() == "Temperature")
                {                    
                    temperatureStrings.Add(temp.DisplayUomName);
                }                
            }
            return temperatureStrings;
        }


        //*********************************************************************


        // get a list of Food Categories
        public List<string> GetFoodCategories()
        {
            var foodCategories = from fc in _recipeContext.FoodCategories.OrderBy(x => x.FoodCategoryName)
                                select fc.FoodCategoryName;                                 

            return foodCategories.ToList();
        }


        //*********************************************************************


        // get a FoodCategory's Id from its Name
        public int GetFoodCategoryIdFromName(string FoodCatName)
        {
            int foodCategory = 0;
            var foodCategories = from fc in _recipeContext.FoodCategories
                            select fc;

            foreach(var fcat in foodCategories)
            {
                if(fcat.FoodCategoryName.Trim() == FoodCatName.Trim())
                {
                    foodCategory = fcat.FoodCategoryId;
                    break;
                }
            }

            return foodCategory;
        }


        //*********************************************************************

        public string CheckNewIngredient(string newIngredientSingularName, string newIngredientPluralName,
            string newIngredientFoodCategoryName, string dryOrWetMeasureName)
        {

            Boolean validationSuccess = true;
            Boolean addIngredient = false;

            if(String.IsNullOrEmpty(newIngredientSingularName)) {validationSuccess = false;}
            if(String.IsNullOrEmpty(newIngredientPluralName)) {validationSuccess = false;}
            if(String.IsNullOrEmpty(newIngredientFoodCategoryName)) {validationSuccess = false;}
            if(String.IsNullOrEmpty(dryOrWetMeasureName)) {validationSuccess = false;}                        

            if(CheckIfIngredientAlreadyExists(newIngredientSingularName, "singular") == true)
                {validationSuccess = false;}
            if(CheckIfIngredientAlreadyExists(newIngredientPluralName, "plural") == true)
                {validationSuccess = false;}

            if(validationSuccess == true)
            {
                addIngredient = AddNewIngredient(newIngredientSingularName, newIngredientPluralName,
                    newIngredientFoodCategoryName, dryOrWetMeasureName);
            }

            if(addIngredient == true){
                return (newIngredientSingularName);
            }else{
                string notAdded = "IngredientNotAdded";
                return (notAdded);
            }
        }


        //*********************************************************************

        public Boolean CheckIfIngredientAlreadyExists(string ingredientName, string scope)
        {
            Boolean exists = false;
            IEnumerable<Ingredient> ingredient = new List<Ingredient>();

            if (scope == "singular")
            {
                ingredient = from i in _recipeContext.Ingredients
                        where i.NameSingular == ingredientName
                        select i;
            }
            else if (scope == "plural")
            {
                ingredient = from i in _recipeContext.Ingredients
                        where i.NamePlural == ingredientName
                        select i;
            }

            if (ingredient.Count() > 0)
            {
                exists = true;
            }

            return exists;
        }


        //*********************************************************************


        // Add a new ingredient
        public Boolean AddNewIngredient(string newIngredientSingularName, string newIngredientPluralName,
            string newIngredientFoodCategoryName, string dryOrWetMeasureName)
        {

            Boolean newIngredientAdded = false;
            Ingredient ingred = new Ingredient();

            char[] newIngredientSingularNameArray = newIngredientSingularName.ToCharArray();
            newIngredientSingularName = newIngredientSingularNameArray[0].ToString().ToUpper();
            for(int x = 1; x < newIngredientSingularNameArray.Length; x++)
            {
                newIngredientSingularName += newIngredientSingularNameArray[x];
            }
            ingred.NameSingular = newIngredientSingularName;

            char[] newIngredientPluralNameArray = newIngredientPluralName.ToCharArray();
            newIngredientPluralName = newIngredientPluralNameArray[0].ToString().ToUpper();
            for (int x = 1; x < newIngredientPluralNameArray.Length; x++)
            {
                newIngredientPluralName += newIngredientPluralNameArray[x];
            }
            ingred.NamePlural = newIngredientPluralName;

            ingred.FoodCategoryId = GetFoodCategoryIdFromName(newIngredientFoodCategoryName);
            ingred.TypeOfMeasureName = dryOrWetMeasureName;
            ingred.ReviewedStatus = false;
            ingred.OneGramToTeaspoonUK = 0.2;
            ingred.OneTeaspoonUKToGram = 5;

            Boolean singularExists = CheckIfIngredientAlreadyExists(newIngredientSingularName, "singular");
            Boolean pluralExists = CheckIfIngredientAlreadyExists(newIngredientPluralName, "plural");

            if ((string.IsNullOrEmpty(ingred.NameSingular) == false) && (string.IsNullOrEmpty(ingred.NamePlural) == false)
                && (string.IsNullOrEmpty(ingred.FoodCategoryId.ToString()) == false)
                && (string.IsNullOrEmpty(ingred.TypeOfMeasureName) == false)
                && (singularExists == false) && (pluralExists == false))
            {

                _recipeContext.Add(ingred);
                int success = _recipeContext.SaveChanges();
                if (success == 1)
                {
                    newIngredientAdded = true;
                }
            }

            return newIngredientAdded;
        }


        //*********************************************************************

        public void DeleteRecipeImageOrphans ()
        {
            IQueryable<Recipe> recipeList = _recipeContext.Recipes;
            IQueryable<RecipeImage> imageList = _recipeContext.RecipeImages;

            List<string> recipeIdents = new List<string>();
            List<string> imageIdents = new List<string>();
            List<string> nomatches = new List<string>();
            foreach(var rec in recipeList) { recipeIdents.Add(rec.RecipeIdent); }
            foreach (var img in imageList) { imageIdents.Add(img.RecipeIdent); }

            foreach(var ident in imageIdents)
            {
                var matchFound = false;
                foreach(var rident in recipeIdents) {
                    if (ident.Equals(rident)) {
                        matchFound = true;
                        break;
                    }
                }
                if (matchFound == false) {
                    nomatches.Add(ident);
                }
            }

            if(nomatches.Count() > 0) {
                foreach(var recipeimageid in nomatches) {
                    var recimage = _recipeContext.RecipeImages.FirstOrDefault(i => i.RecipeIdent.Equals(recipeimageid));
                    _recipeContext.RecipeImages.Remove(recimage);
                    _recipeContext.SaveChangesAsync();
                }
            }
        }


        //*********************************************************************

        public int IncrementUserLikes(string RecipeIdent)
        {
            Recipe recep = _recipeContext.Recipes.FirstOrDefault(r => r.RecipeIdent == RecipeIdent);
            recep.UserLikes++;
            _recipeContext.SaveChanges();

            return (int)recep.UserLikes;
        }


        public double GetTheNewUoMAmountValue(string FullAmount, string NewUoMValue)
        {
            double updateValue = 0.0;
            return updateValue = Math.Round(ConvertFromBaseToOrigUoM(Convert.ToDouble(FullAmount), NewUoMValue), 2, MidpointRounding.AwayFromZero);           
        }


    } // close class
}  // close namespace
