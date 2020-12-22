///delete from RecipeIngredients
///delete from RecipeInstructions
///delete from Recipes


///DBCC CHECKIDENT ('[RecipeImages]', RESEED, 0);
///GO
///DBCC CHECKIDENT ('[RecipeIngredients]', RESEED, 0);
///GO
///DBCC CHECKIDENT ('[RecipeImages]', RESEED, 0);
///GO
///DBCC CHECKIDENT ('[Recipes]', RESEED, 0);
///GO

///delete from RecipeImages where ImageId = 4


///////////////////////////////////////////////////////////////

///delete from Ingredients
///delete from FoodCategories
///delete from UnitsOfMeasures

DBCC CHECKIDENT ('[UnitsOfMeasures]', RESEED, 0);
GO
