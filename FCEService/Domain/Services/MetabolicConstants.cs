namespace FCEService.Domain.Services
{
   
    internal static class MetabolicConstants
    {
     
        public const double WeightCoefficient = 10d;    // kcal per kg body weight
        public const double HeightCoefficient = 6.25d;  // kcal per cm height
        public const double AgeCoefficient = 5d;        // kcal subtracted per year of age
        public const double MaleConstant = 5d;          // BMR = base + 5 (male)
        public const double FemaleConstant = -161d;     // BMR = base - 161 (female)

      
        public const double RookieActivityFactor = 1.200d;
        public const double BeginnerActivityFactor = 1.375d;
        public const double IntermediateActivityFactor = 1.550d;
        public const double AdvanceActivityFactor = 1.725d;
        public const double TrueBeastActivityFactor = 1.900d;

        
        public const double LoseWeightDelta = -500d;
        public const double GainWeightDelta = 300d;
        public const double GainMoreFlexibleDelta = 150d;
       

        public const double WeakStatusMaxThreshold = 1800d;
        public const double NormalStatusMaxThreshold = 2500d;

        public const int RoundingDecimalPlaces = 2;
    }
}