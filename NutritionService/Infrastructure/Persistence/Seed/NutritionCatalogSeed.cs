using NutritionService.Domain.Entities;

namespace NutritionService.Infrastructure.Persistence.Seed
{
    /// <summary>
    /// Reference data for the meal catalog. Meals and meal plans are global (not
    /// per-user), so an empty table means every recommendation request comes back
    /// empty - including the SmartCoach home feed. Applied through HasData so the rows
    /// ship with the migration and land identically on a fresh dev box, CI and Docker.
    ///
    /// The *Json columns are parsed with JsonDocument.Parse when a meal is read
    /// (see GetMealDetailsHandler), so every value here must be valid JSON or the
    /// details endpoint throws.
    ///
    /// Breakfast meals come first by MealId because GetMealRecommendations defaults to
    /// MealType.Breakfast and orders by MealId - the home feed calls it without a type.
    /// </summary>
    public static class NutritionCatalogSeed
    {
        public static IEnumerable<Meal> GetMeals() => new[]
        {
            new Meal
            {
                MealId = 1, Name = "Greek Yogurt Parfait", Type = MealType.Breakfast,
                Calories = 320, Protein = 24, Carbs = 38, Fats = 8,
                IngredientsJson = """["200g Greek yogurt","60g mixed berries","30g granola","1 tsp honey"]""",
                InstructionsJson = """["Spoon half the yogurt into a glass.","Add half the berries and granola.","Repeat the layers and finish with honey."]""",
                VariationsJson = """[{"name":"Vegan","swap":"Use coconut yogurt and maple syrup."}]""",
                AllergensJson = """["dairy","nuts","gluten"]""",
                TagsJson = """["high-protein","quick","vegetarian"]"""
            },
            new Meal
            {
                MealId = 2, Name = "Veggie Omelette", Type = MealType.Breakfast,
                Calories = 380, Protein = 28, Carbs = 9, Fats = 26,
                IngredientsJson = """["3 eggs","40g spinach","50g mushrooms","30g feta","1 tsp olive oil"]""",
                InstructionsJson = """["Saute the mushrooms and spinach in the oil.","Pour over the beaten eggs and cook until just set.","Scatter the feta, fold and serve."]""",
                VariationsJson = """[{"name":"Dairy-free","swap":"Drop the feta and add half an avocado."}]""",
                AllergensJson = """["eggs","dairy"]""",
                TagsJson = """["low-carb","high-protein","vegetarian","keto"]"""
            },
            new Meal
            {
                MealId = 3, Name = "Overnight Oats with Berries", Type = MealType.Breakfast,
                Calories = 410, Protein = 16, Carbs = 62, Fats = 11,
                IngredientsJson = """["60g rolled oats","200ml milk","1 tbsp chia seeds","80g mixed berries","1 tsp honey"]""",
                InstructionsJson = """["Stir the oats, milk and chia seeds together in a jar.","Refrigerate overnight.","Top with berries and honey before eating."]""",
                VariationsJson = """[{"name":"Vegan","swap":"Use oat milk and maple syrup."}]""",
                AllergensJson = """["gluten","dairy"]""",
                TagsJson = """["make-ahead","high-fibre","vegetarian"]"""
            },
            new Meal
            {
                MealId = 4, Name = "Avocado Toast with Poached Eggs", Type = MealType.Breakfast,
                Calories = 450, Protein = 22, Carbs = 38, Fats = 24,
                IngredientsJson = """["2 slices wholegrain bread","1 avocado","2 eggs","Chilli flakes","Lemon juice"]""",
                InstructionsJson = """["Toast the bread.","Mash the avocado with lemon juice and spread it on the toast.","Poach the eggs for three minutes and set them on top.","Finish with chilli flakes."]""",
                VariationsJson = null,
                AllergensJson = """["eggs","gluten"]""",
                TagsJson = """["balanced","vegetarian"]"""
            },
            new Meal
            {
                MealId = 5, Name = "Protein Banana Pancakes", Type = MealType.Breakfast,
                Calories = 520, Protein = 32, Carbs = 66, Fats = 14,
                IngredientsJson = """["1 banana","2 eggs","40g oat flour","1 scoop whey protein","1 tsp baking powder"]""",
                InstructionsJson = """["Blend every ingredient into a smooth batter.","Cook small pancakes over a medium heat, about two minutes a side.","Stack and serve with fruit."]""",
                VariationsJson = """[{"name":"Vegan","swap":"Use plant protein and two flax eggs."}]""",
                AllergensJson = """["eggs","dairy","gluten"]""",
                TagsJson = """["high-protein","post-workout"]"""
            },

            new Meal
            {
                MealId = 6, Name = "Lentil & Vegetable Soup", Type = MealType.Lunch,
                Calories = 380, Protein = 20, Carbs = 55, Fats = 8,
                IngredientsJson = """["150g red lentils","1 carrot","1 onion","2 celery sticks","800ml vegetable stock","1 tsp cumin"]""",
                InstructionsJson = """["Soften the diced vegetables in a large pan.","Add the lentils, stock and cumin.","Simmer for 25 minutes, then blend to your preferred texture."]""",
                VariationsJson = """[{"name":"Higher protein","swap":"Stir in 100g shredded chicken."}]""",
                AllergensJson = """["celery"]""",
                TagsJson = """["vegan","high-fibre","meal-prep"]"""
            },
            new Meal
            {
                MealId = 7, Name = "Tuna Nicoise Salad", Type = MealType.Lunch,
                Calories = 480, Protein = 38, Carbs = 24, Fats = 26,
                IngredientsJson = """["120g tuna steak","2 boiled eggs","100g green beans","150g new potatoes","Olives","Vinaigrette"]""",
                InstructionsJson = """["Boil the potatoes and blanch the beans.","Sear the tuna for two minutes a side and slice it.","Assemble everything and dress with the vinaigrette."]""",
                VariationsJson = null,
                AllergensJson = """["fish","eggs"]""",
                TagsJson = """["high-protein","mediterranean"]"""
            },
            new Meal
            {
                MealId = 8, Name = "Turkey & Hummus Wrap", Type = MealType.Lunch,
                Calories = 520, Protein = 34, Carbs = 55, Fats = 18,
                IngredientsJson = """["1 wholewheat tortilla","120g sliced turkey breast","3 tbsp hummus","Mixed salad leaves","1/2 red pepper"]""",
                InstructionsJson = """["Spread the hummus over the tortilla.","Layer the turkey, leaves and sliced pepper down the middle.","Roll tightly and slice in half."]""",
                VariationsJson = """[{"name":"Vegetarian","swap":"Replace the turkey with falafel."}]""",
                AllergensJson = """["gluten","sesame"]""",
                TagsJson = """["quick","portable","high-protein"]"""
            },
            new Meal
            {
                MealId = 9, Name = "Grilled Chicken Quinoa Bowl", Type = MealType.Lunch,
                Calories = 560, Protein = 45, Carbs = 52, Fats = 16,
                IngredientsJson = """["150g chicken breast","80g quinoa","1/2 avocado","100g roasted vegetables","Lemon dressing"]""",
                InstructionsJson = """["Cook the quinoa in salted water.","Grill the seasoned chicken for six minutes a side and rest it.","Build the bowl and finish with the dressing."]""",
                VariationsJson = """[{"name":"Vegan","swap":"Use 150g marinated tofu."}]""",
                AllergensJson = """[]""",
                TagsJson = """["high-protein","meal-prep","gluten-free"]"""
            },

            new Meal
            {
                MealId = 10, Name = "Chickpea Curry with Brown Rice", Type = MealType.Dinner,
                Calories = 540, Protein = 22, Carbs = 78, Fats = 16,
                IngredientsJson = """["400g chickpeas","200ml coconut milk","400g chopped tomatoes","2 tbsp curry paste","80g brown rice"]""",
                InstructionsJson = """["Fry the curry paste for a minute.","Add the tomatoes, coconut milk and chickpeas, then simmer for 20 minutes.","Serve over the cooked brown rice."]""",
                VariationsJson = """[{"name":"Higher protein","swap":"Add 100g paneer or tofu."}]""",
                AllergensJson = """[]""",
                TagsJson = """["vegan","high-fibre","batch-cook"]"""
            },
            new Meal
            {
                MealId = 11, Name = "Lean Beef Stir-Fry", Type = MealType.Dinner,
                Calories = 580, Protein = 44, Carbs = 48, Fats = 22,
                IngredientsJson = """["150g lean beef strips","150g mixed stir-fry vegetables","80g rice noodles","2 tbsp soy sauce","1 tsp sesame oil","Garlic and ginger"]""",
                InstructionsJson = """["Sear the beef in a very hot wok and set it aside.","Stir-fry the vegetables with the garlic and ginger.","Return the beef, add the noodles and sauce, and toss to coat."]""",
                VariationsJson = null,
                AllergensJson = """["soy","sesame","gluten"]""",
                TagsJson = """["high-protein","quick"]"""
            },
            new Meal
            {
                MealId = 12, Name = "Baked Salmon with Asparagus", Type = MealType.Dinner,
                Calories = 610, Protein = 46, Carbs = 18, Fats = 38,
                IngredientsJson = """["180g salmon fillet","200g asparagus","1 tbsp olive oil","1 lemon","Fresh dill"]""",
                InstructionsJson = """["Heat the oven to 200C.","Lay the salmon and asparagus on a tray, then drizzle with oil and lemon.","Bake for 15 minutes and finish with dill."]""",
                VariationsJson = """[{"name":"Higher carb","swap":"Add 150g roasted new potatoes."}]""",
                AllergensJson = """["fish"]""",
                TagsJson = """["omega-3","low-carb","gluten-free"]"""
            },
            new Meal
            {
                MealId = 13, Name = "Chicken Breast with Sweet Potato", Type = MealType.Dinner,
                Calories = 620, Protein = 52, Carbs = 60, Fats = 14,
                IngredientsJson = """["180g chicken breast","250g sweet potato","150g broccoli","1 tbsp olive oil","Paprika"]""",
                InstructionsJson = """["Roast the cubed sweet potato with oil and paprika for 30 minutes.","Pan-sear the chicken, then finish it in the oven.","Steam the broccoli and plate everything together."]""",
                VariationsJson = null,
                AllergensJson = """[]""",
                TagsJson = """["high-protein","muscle-gain","gluten-free"]"""
            },

            new Meal
            {
                MealId = 14, Name = "Cottage Cheese with Pineapple", Type = MealType.Snack,
                Calories = 190, Protein = 22, Carbs = 18, Fats = 3,
                IngredientsJson = """["150g cottage cheese","80g fresh pineapple"]""",
                InstructionsJson = """["Spoon the cottage cheese into a bowl.","Top with the diced pineapple."]""",
                VariationsJson = null,
                AllergensJson = """["dairy"]""",
                TagsJson = """["high-protein","low-fat","quick"]"""
            },
            new Meal
            {
                MealId = 15, Name = "Protein Shake", Type = MealType.Snack,
                Calories = 210, Protein = 30, Carbs = 12, Fats = 4,
                IngredientsJson = """["1 scoop whey protein","300ml milk","1/2 banana"]""",
                InstructionsJson = """["Blend everything until smooth.","Drink within 30 minutes of training."]""",
                VariationsJson = """[{"name":"Vegan","swap":"Use pea protein and oat milk."}]""",
                AllergensJson = """["dairy"]""",
                TagsJson = """["post-workout","high-protein","quick"]"""
            },
            new Meal
            {
                MealId = 16, Name = "Almonds & Apple", Type = MealType.Snack,
                Calories = 240, Protein = 8, Carbs = 24, Fats = 14,
                IngredientsJson = """["30g almonds","1 apple"]""",
                InstructionsJson = """["Slice the apple.","Serve with the almonds."]""",
                VariationsJson = null,
                AllergensJson = """["nuts"]""",
                TagsJson = """["vegan","portable","high-fibre"]"""
            },
        };

        public static IEnumerable<MealPlan> GetMealPlans() => new[]
        {
            new MealPlan { MealPlanId = 1, Name = "Lean & Light", Description = "A lower-calorie plan built around high-protein, high-fibre meals for a steady deficit.", TargetCalorieRangeMin = 1400, TargetCalorieRangeMax = 1800 },
            new MealPlan { MealPlanId = 2, Name = "Balanced Everyday", Description = "A moderate plan that covers maintenance calories for most moderately active adults.", TargetCalorieRangeMin = 1801, TargetCalorieRangeMax = 2200 },
            new MealPlan { MealPlanId = 3, Name = "Active Performance", Description = "Higher carbohydrate intake to support four or more training sessions a week.", TargetCalorieRangeMin = 2201, TargetCalorieRangeMax = 2600 },
            new MealPlan { MealPlanId = 4, Name = "Muscle Gain", Description = "A calorie surplus with protein spread evenly across the day to support hypertrophy.", TargetCalorieRangeMin = 2601, TargetCalorieRangeMax = 3200 },
        };

        public static IEnumerable<MealPlanItem> GetMealPlanItems() => new[]
        {
            // 1 - Lean & Light (~1430 kcal)
            new MealPlanItem { Id = 1, MealPlanId = 1, MealId = 1, DayOfWeek = "Monday", MealTime = "Breakfast" },
            new MealPlanItem { Id = 2, MealPlanId = 1, MealId = 6, DayOfWeek = "Monday", MealTime = "Lunch" },
            new MealPlanItem { Id = 3, MealPlanId = 1, MealId = 12, DayOfWeek = "Monday", MealTime = "Dinner" },
            new MealPlanItem { Id = 4, MealPlanId = 1, MealId = 14, DayOfWeek = "Monday", MealTime = "Snack" },

            // 2 - Balanced Everyday (~1960 kcal)
            new MealPlanItem { Id = 5, MealPlanId = 2, MealId = 4, DayOfWeek = "Monday", MealTime = "Breakfast" },
            new MealPlanItem { Id = 6, MealPlanId = 2, MealId = 8, DayOfWeek = "Monday", MealTime = "Lunch" },
            new MealPlanItem { Id = 7, MealPlanId = 2, MealId = 13, DayOfWeek = "Monday", MealTime = "Dinner" },
            new MealPlanItem { Id = 8, MealPlanId = 2, MealId = 15, DayOfWeek = "Monday", MealTime = "Snack" },

            // 3 - Active Performance (~2330 kcal)
            new MealPlanItem { Id = 9, MealPlanId = 3, MealId = 3, DayOfWeek = "Monday", MealTime = "Breakfast" },
            new MealPlanItem { Id = 10, MealPlanId = 3, MealId = 9, DayOfWeek = "Monday", MealTime = "Lunch" },
            new MealPlanItem { Id = 11, MealPlanId = 3, MealId = 11, DayOfWeek = "Monday", MealTime = "Dinner" },
            new MealPlanItem { Id = 12, MealPlanId = 3, MealId = 15, DayOfWeek = "Monday", MealTime = "Snack" },
            new MealPlanItem { Id = 13, MealPlanId = 3, MealId = 16, DayOfWeek = "Monday", MealTime = "Evening Snack" },

            // 4 - Muscle Gain (~2680 kcal)
            new MealPlanItem { Id = 14, MealPlanId = 4, MealId = 5, DayOfWeek = "Monday", MealTime = "Breakfast" },
            new MealPlanItem { Id = 15, MealPlanId = 4, MealId = 9, DayOfWeek = "Monday", MealTime = "Lunch" },
            new MealPlanItem { Id = 16, MealPlanId = 4, MealId = 13, DayOfWeek = "Monday", MealTime = "Dinner" },
            new MealPlanItem { Id = 17, MealPlanId = 4, MealId = 15, DayOfWeek = "Monday", MealTime = "Snack" },
            new MealPlanItem { Id = 18, MealPlanId = 4, MealId = 16, DayOfWeek = "Monday", MealTime = "Evening Snack" },
        };
    }
}
