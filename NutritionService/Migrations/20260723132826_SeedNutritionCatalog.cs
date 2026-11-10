using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NutritionService.Migrations
{
    /// <inheritdoc />
    public partial class SeedNutritionCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MealPlans",
                columns: new[] { "MealPlanId", "Description", "Name", "TargetCalorieRangeMax", "TargetCalorieRangeMin" },
                values: new object[,]
                {
                    { 1, "A lower-calorie plan built around high-protein, high-fibre meals for a steady deficit.", "Lean & Light", 1800.0, 1400.0 },
                    { 2, "A moderate plan that covers maintenance calories for most moderately active adults.", "Balanced Everyday", 2200.0, 1801.0 },
                    { 3, "Higher carbohydrate intake to support four or more training sessions a week.", "Active Performance", 2600.0, 2201.0 },
                    { 4, "A calorie surplus with protein spread evenly across the day to support hypertrophy.", "Muscle Gain", 3200.0, 2601.0 }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "MealId", "AllergensJson", "Calories", "Carbs", "Fats", "IngredientsJson", "InstructionsJson", "Name", "Protein", "TagsJson", "Type", "VariationsJson" },
                values: new object[,]
                {
                    { 1, "[\"dairy\",\"nuts\",\"gluten\"]", 320.0, 38.0, 8.0, "[\"200g Greek yogurt\",\"60g mixed berries\",\"30g granola\",\"1 tsp honey\"]", "[\"Spoon half the yogurt into a glass.\",\"Add half the berries and granola.\",\"Repeat the layers and finish with honey.\"]", "Greek Yogurt Parfait", 24.0, "[\"high-protein\",\"quick\",\"vegetarian\"]", 0, "[{\"name\":\"Vegan\",\"swap\":\"Use coconut yogurt and maple syrup.\"}]" },
                    { 2, "[\"eggs\",\"dairy\"]", 380.0, 9.0, 26.0, "[\"3 eggs\",\"40g spinach\",\"50g mushrooms\",\"30g feta\",\"1 tsp olive oil\"]", "[\"Saute the mushrooms and spinach in the oil.\",\"Pour over the beaten eggs and cook until just set.\",\"Scatter the feta, fold and serve.\"]", "Veggie Omelette", 28.0, "[\"low-carb\",\"high-protein\",\"vegetarian\",\"keto\"]", 0, "[{\"name\":\"Dairy-free\",\"swap\":\"Drop the feta and add half an avocado.\"}]" },
                    { 3, "[\"gluten\",\"dairy\"]", 410.0, 62.0, 11.0, "[\"60g rolled oats\",\"200ml milk\",\"1 tbsp chia seeds\",\"80g mixed berries\",\"1 tsp honey\"]", "[\"Stir the oats, milk and chia seeds together in a jar.\",\"Refrigerate overnight.\",\"Top with berries and honey before eating.\"]", "Overnight Oats with Berries", 16.0, "[\"make-ahead\",\"high-fibre\",\"vegetarian\"]", 0, "[{\"name\":\"Vegan\",\"swap\":\"Use oat milk and maple syrup.\"}]" },
                    { 4, "[\"eggs\",\"gluten\"]", 450.0, 38.0, 24.0, "[\"2 slices wholegrain bread\",\"1 avocado\",\"2 eggs\",\"Chilli flakes\",\"Lemon juice\"]", "[\"Toast the bread.\",\"Mash the avocado with lemon juice and spread it on the toast.\",\"Poach the eggs for three minutes and set them on top.\",\"Finish with chilli flakes.\"]", "Avocado Toast with Poached Eggs", 22.0, "[\"balanced\",\"vegetarian\"]", 0, null },
                    { 5, "[\"eggs\",\"dairy\",\"gluten\"]", 520.0, 66.0, 14.0, "[\"1 banana\",\"2 eggs\",\"40g oat flour\",\"1 scoop whey protein\",\"1 tsp baking powder\"]", "[\"Blend every ingredient into a smooth batter.\",\"Cook small pancakes over a medium heat, about two minutes a side.\",\"Stack and serve with fruit.\"]", "Protein Banana Pancakes", 32.0, "[\"high-protein\",\"post-workout\"]", 0, "[{\"name\":\"Vegan\",\"swap\":\"Use plant protein and two flax eggs.\"}]" },
                    { 6, "[\"celery\"]", 380.0, 55.0, 8.0, "[\"150g red lentils\",\"1 carrot\",\"1 onion\",\"2 celery sticks\",\"800ml vegetable stock\",\"1 tsp cumin\"]", "[\"Soften the diced vegetables in a large pan.\",\"Add the lentils, stock and cumin.\",\"Simmer for 25 minutes, then blend to your preferred texture.\"]", "Lentil & Vegetable Soup", 20.0, "[\"vegan\",\"high-fibre\",\"meal-prep\"]", 1, "[{\"name\":\"Higher protein\",\"swap\":\"Stir in 100g shredded chicken.\"}]" },
                    { 7, "[\"fish\",\"eggs\"]", 480.0, 24.0, 26.0, "[\"120g tuna steak\",\"2 boiled eggs\",\"100g green beans\",\"150g new potatoes\",\"Olives\",\"Vinaigrette\"]", "[\"Boil the potatoes and blanch the beans.\",\"Sear the tuna for two minutes a side and slice it.\",\"Assemble everything and dress with the vinaigrette.\"]", "Tuna Nicoise Salad", 38.0, "[\"high-protein\",\"mediterranean\"]", 1, null },
                    { 8, "[\"gluten\",\"sesame\"]", 520.0, 55.0, 18.0, "[\"1 wholewheat tortilla\",\"120g sliced turkey breast\",\"3 tbsp hummus\",\"Mixed salad leaves\",\"1/2 red pepper\"]", "[\"Spread the hummus over the tortilla.\",\"Layer the turkey, leaves and sliced pepper down the middle.\",\"Roll tightly and slice in half.\"]", "Turkey & Hummus Wrap", 34.0, "[\"quick\",\"portable\",\"high-protein\"]", 1, "[{\"name\":\"Vegetarian\",\"swap\":\"Replace the turkey with falafel.\"}]" },
                    { 9, "[]", 560.0, 52.0, 16.0, "[\"150g chicken breast\",\"80g quinoa\",\"1/2 avocado\",\"100g roasted vegetables\",\"Lemon dressing\"]", "[\"Cook the quinoa in salted water.\",\"Grill the seasoned chicken for six minutes a side and rest it.\",\"Build the bowl and finish with the dressing.\"]", "Grilled Chicken Quinoa Bowl", 45.0, "[\"high-protein\",\"meal-prep\",\"gluten-free\"]", 1, "[{\"name\":\"Vegan\",\"swap\":\"Use 150g marinated tofu.\"}]" },
                    { 10, "[]", 540.0, 78.0, 16.0, "[\"400g chickpeas\",\"200ml coconut milk\",\"400g chopped tomatoes\",\"2 tbsp curry paste\",\"80g brown rice\"]", "[\"Fry the curry paste for a minute.\",\"Add the tomatoes, coconut milk and chickpeas, then simmer for 20 minutes.\",\"Serve over the cooked brown rice.\"]", "Chickpea Curry with Brown Rice", 22.0, "[\"vegan\",\"high-fibre\",\"batch-cook\"]", 2, "[{\"name\":\"Higher protein\",\"swap\":\"Add 100g paneer or tofu.\"}]" },
                    { 11, "[\"soy\",\"sesame\",\"gluten\"]", 580.0, 48.0, 22.0, "[\"150g lean beef strips\",\"150g mixed stir-fry vegetables\",\"80g rice noodles\",\"2 tbsp soy sauce\",\"1 tsp sesame oil\",\"Garlic and ginger\"]", "[\"Sear the beef in a very hot wok and set it aside.\",\"Stir-fry the vegetables with the garlic and ginger.\",\"Return the beef, add the noodles and sauce, and toss to coat.\"]", "Lean Beef Stir-Fry", 44.0, "[\"high-protein\",\"quick\"]", 2, null },
                    { 12, "[\"fish\"]", 610.0, 18.0, 38.0, "[\"180g salmon fillet\",\"200g asparagus\",\"1 tbsp olive oil\",\"1 lemon\",\"Fresh dill\"]", "[\"Heat the oven to 200C.\",\"Lay the salmon and asparagus on a tray, then drizzle with oil and lemon.\",\"Bake for 15 minutes and finish with dill.\"]", "Baked Salmon with Asparagus", 46.0, "[\"omega-3\",\"low-carb\",\"gluten-free\"]", 2, "[{\"name\":\"Higher carb\",\"swap\":\"Add 150g roasted new potatoes.\"}]" },
                    { 13, "[]", 620.0, 60.0, 14.0, "[\"180g chicken breast\",\"250g sweet potato\",\"150g broccoli\",\"1 tbsp olive oil\",\"Paprika\"]", "[\"Roast the cubed sweet potato with oil and paprika for 30 minutes.\",\"Pan-sear the chicken, then finish it in the oven.\",\"Steam the broccoli and plate everything together.\"]", "Chicken Breast with Sweet Potato", 52.0, "[\"high-protein\",\"muscle-gain\",\"gluten-free\"]", 2, null },
                    { 14, "[\"dairy\"]", 190.0, 18.0, 3.0, "[\"150g cottage cheese\",\"80g fresh pineapple\"]", "[\"Spoon the cottage cheese into a bowl.\",\"Top with the diced pineapple.\"]", "Cottage Cheese with Pineapple", 22.0, "[\"high-protein\",\"low-fat\",\"quick\"]", 3, null },
                    { 15, "[\"dairy\"]", 210.0, 12.0, 4.0, "[\"1 scoop whey protein\",\"300ml milk\",\"1/2 banana\"]", "[\"Blend everything until smooth.\",\"Drink within 30 minutes of training.\"]", "Protein Shake", 30.0, "[\"post-workout\",\"high-protein\",\"quick\"]", 3, "[{\"name\":\"Vegan\",\"swap\":\"Use pea protein and oat milk.\"}]" },
                    { 16, "[\"nuts\"]", 240.0, 24.0, 14.0, "[\"30g almonds\",\"1 apple\"]", "[\"Slice the apple.\",\"Serve with the almonds.\"]", "Almonds & Apple", 8.0, "[\"vegan\",\"portable\",\"high-fibre\"]", 3, null }
                });

            migrationBuilder.InsertData(
                table: "MealPlanItems",
                columns: new[] { "Id", "DayOfWeek", "MealId", "MealPlanId", "MealTime" },
                values: new object[,]
                {
                    { 1, "Monday", 1, 1, "Breakfast" },
                    { 2, "Monday", 6, 1, "Lunch" },
                    { 3, "Monday", 12, 1, "Dinner" },
                    { 4, "Monday", 14, 1, "Snack" },
                    { 5, "Monday", 4, 2, "Breakfast" },
                    { 6, "Monday", 8, 2, "Lunch" },
                    { 7, "Monday", 13, 2, "Dinner" },
                    { 8, "Monday", 15, 2, "Snack" },
                    { 9, "Monday", 3, 3, "Breakfast" },
                    { 10, "Monday", 9, 3, "Lunch" },
                    { 11, "Monday", 11, 3, "Dinner" },
                    { 12, "Monday", 15, 3, "Snack" },
                    { 13, "Monday", 16, 3, "Evening Snack" },
                    { 14, "Monday", 5, 4, "Breakfast" },
                    { 15, "Monday", 9, 4, "Lunch" },
                    { 16, "Monday", 13, 4, "Dinner" },
                    { 17, "Monday", 15, 4, "Snack" },
                    { 18, "Monday", 16, 4, "Evening Snack" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MealPlanItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "MealPlanId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "MealPlanId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "MealPlanId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MealPlans",
                keyColumn: "MealPlanId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Meals",
                keyColumn: "MealId",
                keyValue: 16);
        }
    }
}
