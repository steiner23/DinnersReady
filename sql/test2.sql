select * from ingredients
where NameSingular = 'Sage'
order by ingredientid DESC;

select * from ingredients
order by IngredientId DESC;


select count(*) from ingredients
where ingredientid > 444;

select * from ingredients
where ingredientid > 444
order by ingredientid DESC;

/// delete from ingredients
where ingredientid > 444;



SELECT FoodCategoryId, DerivedFoodCategoryName, FoodCategoryName
      FROM FoodCategories 
      ORDER BY DerivedFoodCategoryName


      SELECT [f].[FoodCategoryId], [f].[DerivedFoodCategoryName], [f].[FoodCategoryName]
      FROM [FoodCategories] AS [f]
      ORDER BY [f].[DerivedFoodCategoryName]

select * from foodcategories

select f.foodcategoryname, 
f.derivedfoodcategoryname,
count(*) as Count FROM
foodcategories f inner join ingredients i
on f.foodcategoryid = i.foodcategoryid
group by f.derivedfoodcategoryname,
f.foodcategoryname
order by f.derivedfoodcategoryname,
f.foodcategoryname;

select * from recipes

select * from ingredients

select * from ingredients
where NameSingular  ='Bob'

update ingredients set TypeOfMeasureName =
'Dry' where TypeOfMeasureName = 'dry'

select * from UnitsOfMeasures









