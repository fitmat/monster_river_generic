using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YipliUtils
{
    /* ******Gamification*******
    * Function to be called after the gameplay for Report card screen for every game
    * Calculations are aligned to actual cloud functions formulas which gets stored to the player backend
    */
    /*
    public static float GetFitnessPoints(IDictionary<PlayerActions, int> playerActionCounts)
    {
        return PlayerSession.Instance.FitnesssPoints;
    }
    */

    /* ******Gamification*******
    * Function to be called after the gameplay for Report card screen for every game
    * Calculations are aligned to actual cloud functions formulas which gets stored to the player backend
    */
    /*
        public static float GetFitnessPointsWithRandomization(IDictionary<PlayerActions, int> playerActionCounts)
        {
            float fp = 0.0f;
            foreach (KeyValuePair<PlayerActions, int> action in playerActionCounts)
            {
                fp += GetFitnessPointsPerAction(action.Key) * action.Value;
            }
            return fp * Random.Range(0.92f, 1.04f);
        }
    */

    /* ******Gamification*******
    * Function to be called after the gameplay for Experience Points for every game
    * Calculations are aligned to actual cloud functions formulas which gets stored to the player backend
    */
    public static int GetXP(double secs)
    {
        return (int)secs / 10;
    }

    /* ******Gamification*******
     * Function to be called after the gameplay for Report card screen for every game
     * Calculations are aligned to actual cloud functions formulas which gets stored to the player backend
     */
    /*
    public static float GetCaloriesBurned(IDictionary<PlayerActions, int> playerActionCounts)
    {
        float calories = 0.0f;
        foreach (KeyValuePair<PlayerActions, int> action in playerActionCounts)
        {
            calories += GetCaloriesPerAction(action.Key) * action.Value;
        }
        return calories;
    }
    */

    /* 
     * This function returns Yipli Fitness points predeclared for every player Action.
     * Add a new case here with its identified FPs, whenever a new player action.
     * The values are mapped with the cloud functions algorithm to calculate the fitness points.
     * Change this function, if the values in the cloud-function changes.
     */
    public static float GetFitnessPointsPerAction(PlayerActions playerAction)
    {
        Debug.Log("GetFitnessPointsPerAction() called for " + playerAction);
        float fp = 0.0f;
        switch (playerAction)
        {
            case PlayerActions.LEFTMOVE:
                fp = 10.0f;
                break;
            case PlayerActions.RIGHTMOVE:
                fp = 10.0f;
                break;
            case PlayerActions.JUMP:
                fp = 10.0f;
                break;
            case PlayerActions.RUNNING:
                fp = 4.0f;
                break;
            case PlayerActions.JUMPIN:
                fp = 10.0f;
                break;
            case PlayerActions.JUMPOUT:
                fp = 10.0f;
                break;
            case PlayerActions.R_LEG_HOPPING:
                fp = 10.0f;
                break;
            case PlayerActions.L_LEG_HOPPING:
                fp = 10.0f;
                break;
            case PlayerActions.STOP:
                fp = 0.0f;
                break;

            case PlayerActions.RUNNINGSTOPPED:
                fp = 0.0f;
                break;

            case PlayerActions.TILES:
                fp = 10.0f;
                break;

            case PlayerActions.JUMPING_JACK:
                fp = 10.0f;
                break;

            case PlayerActions.SKIER_JACK:
                fp = 10.0f;
                break;

            case PlayerActions.CROSSOVER_JUMPING_JACK:
                fp = 10.0f;
                break;

            case PlayerActions.LUNGES_RUN:
                fp = 10.0f;
                break;

            case PlayerActions.MOUNTAIN_CLIMBING:
                fp = 10.0f;
                break;

            case PlayerActions.PLANK_STARTED:
                fp = 10.0f;
                break;

            case PlayerActions.PLANK_STOPPED:
                fp = 10.0f;
                break;

            case PlayerActions.MULE_KICK:
                fp = 10.0f;
                break;

            case PlayerActions.BURPEE:
                fp = 10.0f;
                break;

            case PlayerActions.JUMPS_180:
                fp = 10.0f;
                break;

            case PlayerActions.DIAGONAL_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.FORWARD_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.BACKWARD_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.RIGHT_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.LEFT_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.STAR_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.CHEST_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.HOPSCOTCH:
                fp = 10.0f;
                break;

            case PlayerActions.BALANCE_STARTED:
                fp = 10.0f;
                break;

            case PlayerActions.BALANCE_STOPPED:
                fp = 10.0f;
                break;

            case PlayerActions.ARM_STARTED_1:
                fp = 10.0f;
                break;

            case PlayerActions.ARM_STOPPED_1:
                fp = 10.0f;
                break;

            case PlayerActions.NINJA_KICK:
                fp = 10.0f;
                break;

            case PlayerActions.HIGH_KNEE:
                fp = 10.0f;
                break;

            case PlayerActions.SQUATS_180:
                fp = 10.0f;
                break;

            case PlayerActions.SQUAT_AND_JUMP:
                fp = 10.0f;
                break;

            case PlayerActions.SQUAT_AND_KICK:
                fp = 10.0f;
                break;

            case PlayerActions.SQUATS:
                fp = 10.0f;
                break;

            case PlayerActions.SQUAT_AND_JUMPING_JACK:
                fp = 10.0f;
                break;

            case PlayerActions.LATERAL_SQUATS:
                fp = 10.0f;
                break;

            case PlayerActions.PLANK_JUMP_INS:
                fp = 10.0f;
                break;

            case PlayerActions.LEG_DOG_3:
                fp = 10.0f;
                break;

            case PlayerActions.BANARSANA:
                fp = 10.0f;
                break;

            case PlayerActions.ARDHA_CHANDRASANA:
                fp = 10.0f;
                break;

            case PlayerActions.MALASANA:
                fp = 10.0f;
                break;

            case PlayerActions.LEFT_TAP:
                fp = 2.0f;
                break;

            case PlayerActions.RIGHT_TAP:
                fp = 2.0f;
                break;

            case PlayerActions.LEFT_TOUCH:
                fp = 2.0f;
                break;

            case PlayerActions.RIGHT_TOUCH:
                fp = 2.0f;
                break;

            default:
                Debug.Log("Invalid action found while calculating the FP. FP returned would be 0.");
                break;
        }
        return fp;
    }


    /* 
    * This class defines all the player actions to be recieved from the FmDriver.
    * Add a new const string here, whenever a new action is added for the player in FmDriver.
    */
    public enum PlayerActions
    {
        LEFT,
        RIGHT,
        ENTER,
        LEFTMOVE,
        RIGHTMOVE,
        LEFT_TAP,
        RIGHT_TAP,
        JUMP,
        STOP,
        RUNNINGSTOPPED,
        RUNNING,
        PAUSE,
        JUMPIN,
        JUMPOUT,
        TILES,
        R_LEG_HOPPING,
        L_LEG_HOPPING,
        JUMPING_JACK, // c
        SKIER_JACK,
        CROSSOVER_JUMPING_JACK,
        LUNGES_RUN, // b
        MOUNTAIN_CLIMBING, // b
        PLANK_STARTED,
        PLANK_STOPPED,
        MULE_KICK, // b
        BURPEE,
        JUMPS_180, // b
        DIAGONAL_JUMP,
        FORWARD_JUMP,
        BACKWARD_JUMP,
        RIGHT_JUMP,
        LEFT_JUMP,
        STAR_JUMP,
        CHEST_JUMP, // c
        HOPSCOTCH,
        BALANCE_STARTED,
        BALANCE_STOPPED,
        ARM_STARTED_1,
        ARM_STOPPED_1,
        NINJA_KICK,
        HIGH_KNEE,
        SQUATS_180, // b
        SQUAT_AND_JUMP, // b
        SQUAT_AND_KICK, // b
        SQUATS,
        SQUAT_AND_JUMPING_JACK, // b
        LATERAL_SQUATS, // b
        PLANK_JUMP_INS, // b
        LEG_DOG_3, // v
        BANARSANA, // v
        AEROPLANE_POSE,
        VIKRASANA, // v
        ARDHA_CHANDRASANA,
        MALASANA, // v
        BASIC1, // basics // Running, Running Stop, High-knee, skeir-jack
        BASIC2, // Running, Running Stop
        BASIC3, // High-knee
        BASIC4, // skier-jack
        LEFT_TOUCH,
        RIGHT_TOUCH,
        INVALID_ACTION,
        TROUBLESHOOTING
    }

    /* 
     * This function returns calories predeclared for every player Action.
     * Add a new case here with its identified calories burnt, whenever a new player action is added.
     * The values are mapped with the cloud functions algorithm to calculate the calories.
     * Change this function, if the values in the cloud-function changes.
     */
    public static float GetCaloriesPerAction(PlayerActions playerAction)
    {
        Debug.Log("GetCaloriesPerAction() called for " + playerAction);
        float calories = 0.0f;
        switch (playerAction)
        {
            case PlayerActions.LEFTMOVE:
                calories = 0.04f;
                break;
            case PlayerActions.RIGHTMOVE:
                calories = 0.04f;
                break;
            case PlayerActions.JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.RUNNING:
                calories = 0.04f;
                break;
            case PlayerActions.JUMPIN:
                calories = 0.1f;
                break;
            case PlayerActions.JUMPOUT:
                calories = 0.1f;
                break;
            case PlayerActions.R_LEG_HOPPING:
                calories = 0.1f;
                break;
            case PlayerActions.L_LEG_HOPPING:
                calories = 0.1f;
                break;
            case PlayerActions.TILES:
                calories = 0.1f;
                break;
            case PlayerActions.JUMPING_JACK:
                calories = 0.1f;
                break;
            case PlayerActions.SKIER_JACK:
                calories = 0.1f;
                break;
            case PlayerActions.CROSSOVER_JUMPING_JACK:
                calories = 0.1f;
                break;
            case PlayerActions.LUNGES_RUN:
                calories = 0.1f;
                break;
            case PlayerActions.MOUNTAIN_CLIMBING:
                calories = 0.1f;
                break;
            case PlayerActions.PLANK_STARTED:
                calories = 0.1f;
                break;
            case PlayerActions.PLANK_STOPPED:
                calories = 0.1f;
                break;
            case PlayerActions.MULE_KICK:
                calories = 0.1f;
                break;
            case PlayerActions.BURPEE:
                calories = 0.1f;
                break;
            case PlayerActions.JUMPS_180:
                calories = 0.1f;
                break;
            case PlayerActions.DIAGONAL_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.FORWARD_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.BACKWARD_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.RIGHT_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.LEFT_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.STAR_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.CHEST_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.HOPSCOTCH:
                calories = 0.1f;
                break;
            case PlayerActions.BALANCE_STARTED:
                calories = 0.1f;
                break;
            case PlayerActions.BALANCE_STOPPED:
                calories = 0.1f;
                break;
            case PlayerActions.ARM_STARTED_1:
                calories = 0.1f;
                break;
            case PlayerActions.ARM_STOPPED_1:
                calories = 0.1f;
                break;
            case PlayerActions.NINJA_KICK:
                calories = 0.1f;
                break;
            case PlayerActions.HIGH_KNEE:
                calories = 0.1f;
                break;
            case PlayerActions.SQUATS_180:
                calories = 0.1f;
                break;
            case PlayerActions.SQUAT_AND_JUMP:
                calories = 0.1f;
                break;
            case PlayerActions.SQUAT_AND_KICK:
                calories = 0.1f;
                break;
            case PlayerActions.SQUATS:
                calories = 0.1f;
                break;
            case PlayerActions.SQUAT_AND_JUMPING_JACK:
                calories = 0.1f;
                break;
            case PlayerActions.LATERAL_SQUATS:
                calories = 0.1f;
                break;
            case PlayerActions.PLANK_JUMP_INS:
                calories = 0.1f;
                break;
            case PlayerActions.LEG_DOG_3:
                calories = 0.1f;
                break;
            case PlayerActions.BANARSANA:
                calories = 0.1f;
                break;
            case PlayerActions.ARDHA_CHANDRASANA:
                calories = 0.1f;
                break;
            case PlayerActions.MALASANA:
                calories = 0.1f;
                break;
            case PlayerActions.LEFT_TAP:
                calories = 0.02f;
                break;
            case PlayerActions.RIGHT_TAP:
                calories = 0.02f;
                break;
            case PlayerActions.LEFT_TOUCH:
                calories = 0.02f;
                break;
            case PlayerActions.RIGHT_TOUCH:
                calories = 0.02f;
                break;
            default:
                Debug.Log("Invalid action found while calculating the calories. Calories returned would be 0.");
                break;
        }
        return calories;
    }

}
