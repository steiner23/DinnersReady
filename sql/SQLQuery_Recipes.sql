select * from Recipes

select * from RecipeIngredients
select * from Ingredients
select * from Recipes

select * from RecipeInstructions
select * from Recipes

select * from RecipeImages
select * from Recipes

select * from UnitsOfMeasures
select * from Ingredients
select * from RecipeInstructions
select * from Recipes

select * from FoodCategories
select * from Ingredients
select * from Recipes


///delete from RecipeIngredients
///delete from RecipeInstructions
///delete from RecipeImages
///delete from Recipes

///dbcc checkident ('[RecipeIngredients]', RESEED, 0);
///dbcc checkident ('[RecipeInstructions]', RESEED, 0);
///dbcc checkident ('[RecipeImages]', RESEED, 0);
///dbcc checkident ('[Recipes]', RESEED, 0);

select * from UnitsOfMeasures
///update UnitsOfMeasures set SystemOfMeasureName = 'Imperial' where UnitOfMeasureId = 12