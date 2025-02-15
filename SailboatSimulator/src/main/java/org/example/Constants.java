package org.example;

public class Constants {

    /**
     * Lift to Drag ratio of water
     */
    public final static double L_TO_D_WATER_RATIO = 15;

    /**
     * Lift to Drag ratio of air
     */
    public final static double L_TO_D_AIR_RATIO = 5;

    /**
     * Parameter which describes the effectiveness of boat in 180 course
     */
    public final static double S0 = 1.5;

    /**
     * Speed of wind in
     * UNIT: knots
     */
    public final static double W_SPEED_WIND = 10;


    /**
     * v* param which represents the angle in which the profile of sailing changes
     * UNIT: radians
     */
    public final static double V_BORDER_RAD = -(Math.atan(L_TO_D_AIR_RATIO) - Math.PI);

    /**
     * v* param which represents the angle in which the profile of sailing changes
     * UNIT: degrees
     */
    public final static double V_BORDER_DEG = V_BORDER_RAD * 180/Math.PI;

}
