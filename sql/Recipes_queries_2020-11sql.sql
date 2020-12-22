select * from Recipes


select * from Recipes where name = 'Beef Lasagne'


select name as 'Name', userlikes as 'Likes', viewcount as 'View'
from Recipes
where name = 'Beef Lasagne'

select * from FoodCategories

select * from RecipeIngredients where RecipeId = 4
select * from Ingredients

select i.NameSingular, r.Amount, r.Comment, r.OriginalUnitOfMeasure from
Ingredients i inner join RecipeIngredients r
on i.IngredientId = r.IngredientId
where r.RecipeId = 4
order by r.RecipeId

select * from RecipeInstructions where RecipeId = 4
order by InstructionStepId

select * from UnitsOfMeasures

///update Recipes set viewcount = 0
///update Recipes set userlikes = 0