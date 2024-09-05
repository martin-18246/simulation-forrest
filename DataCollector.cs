using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataCollector : MonoBehaviour
{
    public static Snapshot MakeSnapshot()
    {
        Snapshot snapshot = new Snapshot();

        return snapshot;
    }
}

public class Snapshot
{
    public float snapshotTakenTime;

    public int numRabbits;
    public int numFoxes;
    public int numTotalAnimals;
    public int numFood;

    public float averageSpeedRabbit;
    public float averageSpeedFox;
    
    public float averageVisionRangeRabbit;
    public float averageVisionRangeFox;

    public Snapshot()
    {
        snapshotTakenTime = Time.time;

        List<Animal> rabbits = (from a in EntityManager.animalList where a.GetType() == typeof(Rabbit) select a).ToList();
        List<Animal> foxes = (from a in EntityManager.animalList where a.GetType() == typeof(Fox) select a).ToList();
        List<Animal> animals = EntityManager.animalList;
        List<IEdible> food = EntityManager.foodList;

        // numbers
        numRabbits = rabbits.Count;
        numFoxes = foxes.Count;
        numTotalAnimals = animals.Count;
        numFood = EntityManager.foodList.Count;

        // averages
        averageSpeedRabbit = CalculateAverageTrait(rabbits, Animal.TraitType.movementSpeed);
        averageSpeedFox = CalculateAverageTrait(foxes, Animal.TraitType.movementSpeed);
        
        averageVisionRangeRabbit = CalculateAverageTrait(rabbits, Animal.TraitType.visionRange);
        averageVisionRangeFox = CalculateAverageTrait(foxes, Animal.TraitType.visionRange);

    }


    public float CalculateAverageTrait(List<Animal> list, Animal.TraitType traitType)
    {
        float total = 0;

        foreach (var entity in list)
        {
            total += entity.GetTrait(traitType);
        }

        return total / list.Count;
    }


    public float GetTrait(InfoPoint infoPoint)
    {
        switch (infoPoint)
        {
            case InfoPoint.NumFoxes:
                return numFoxes;

            case InfoPoint.NumRabbits:
                return numRabbits;

            case InfoPoint.NumFood:
                return numFood;

            case InfoPoint.AverageSpeedRabbit:
                return averageSpeedRabbit;

            case InfoPoint.AverageSpeedFox:
                return averageSpeedFox;
        }

        return 0;
    }

}

public enum InfoPoint
{
    NumFoxes,
    NumRabbits,
    NumTotalAnimals,
    NumFood,
    AverageSpeedRabbit,
    AverageSpeedFox
}
