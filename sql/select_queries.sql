select * from [dbo].[Recipes]
select * from [dbo].[RecipeIngredients]
select * from [dbo].[RecipeInstructions]
select * from [dbo].[RecipeImages]

select * from [dbo].[Ingredients]
select * from [dbo].[FoodCategories]
select * from [dbo].[UnitsOfMeasures]



select r.Name, in.InstructionStepId, in.Description, 
in.TemperatureValue, in.OriginalTemperatureUnitOfMeasure
from RecipeInstructions in inner join Recipe r
where in.RecipeId = 
(select RecipeId from Recipes where Name = 'Beef Lasagne')


select distinct(TemperatureValue) from RecipeInstructions


select i.NameSingular, ri.Amount, ri.OriginalUnitOfMeasure, ri.Comment from RecipeIngredients ri
left join Ingredients i
on ri.IngredientId = i.IngredientId
where ri.RecipeId = 4
order by ri.RecipeIngredientId

select * from RecipeInstructions where RecipeId = 4 order by InstructionStepId



select * from UnitsOfMeasures

// --------------------------------------------------------------------------------------------

select * from [dbo].[Recipes]

select n.RecipeIngredientId, i.NamePlural, n.Amount, n.OriginalunitOfMeasure, n.Comment
from Recipes r inner join RecipeIngredients n on r.RecipeId = n.RecipeId
inner join Ingredients i on n.IngredientId = i.IngredientId
where r.RecipeId = 1
order by n.RecipeIngredientId

select * from RecipeInstructions where RecipeId = 1 order by InstructionStepId


update RecipeIngredients set Amount = 1.25 where RecipeIngredientId = 6
update RecipeIngredients set Amount = 2.5 where RecipeIngredientId = 7
update RecipeIngredients set Amount = 2.5 where RecipeIngredientId = 8
update RecipeIngredients set Amount = 15 where RecipeIngredientId = 13
update RecipeIngredients set Amount = 10 where RecipeIngredientId = 12

update RecipeInstructions set TemperatureValue = 180 where InstructionId = 213
update RecipeInstructions set TemperatureValue = 180 where InstructionId = 206


select * from RecipeIngredients


from [dbo].[RecipeIngredients] where RecipeId = 4


////update Recipes set Description = 'Beef Lasagne' where RecipeId = ?

////DBCC CHECKIDENT ('[RecipeIngred//////]', RESEED, 0);
GO


select [Comment] from [dbo].[RecipeIngredients] where [Comment] = '0'

update RecipeIngredients set Comment = '--none--' where [Comment] = '0'


select [OriginalUnitOfMeasure] from [dbo].[RecipeIngredients]
select distinct([OriginalUnitOfMeasure]) from [dbo].[RecipeIngredients]

update RecipeInstructions set OriginalTemperatureUnitOfMeasure = '[none]'
where OriginalTemperatureUnitOfMeasure = 'none'



select [OriginalTemperatureUnitOfMeasure] from [dbo].[RecipeInstructions]

select distinct([OriginalTemperatureUnitOfMeasure]) from [dbo].[RecipeInstructions]

select [OriginalTemperatureUnitOfMeasure] from [dbo].[RecipeInstructions]
where [OriginalTemperatureUnitOfMeasure] = 'none'

update [dbo].[RecipeInstructions] set [OriginalTemperatureUnitOfMeasure] = '--none--'
where [OriginalTemperatureUnitOfMeasure] = 'none'



