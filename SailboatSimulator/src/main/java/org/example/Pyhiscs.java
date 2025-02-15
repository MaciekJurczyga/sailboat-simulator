package org.example;

import static org.example.Constants.*;

public class Pyhiscs {

    /**
     * variable which describes angle between boat direction and real wind direction
     * UNIT: degrees
     */
    private final double v_deg;

    /**
     * variable which describes angle between boat direction and real wind direction
     * UNIT: radians
     */
    private final double v_rad;

    /**
     * tangents of v_rad
     */
    private final double tan_of_v;


    /**
     * If dead_angle_checker_1 + dead_angle_check_2 == 2, means our boat is NOT in dead angle
     */
    private int dead_angle_checker_1 = 0;
    private int dead_angle_check_2 = 0;

    /**
     * Effectiveness of boat parameter
     */
    private double s_of_v;

    /**
     * Effectiveness of boat parameter, this also includes change of sail type
     * After V_BORDER is crossed.
     */
    private double S_of_v;

    /**
     * Angle of apparent wind
     * UNIT: radians
     */
    private double w_rad;

    /**
     * Angle of apparent wind
     * UNIT: degrees
     */
    private double w_deg;


    /**
     * Boat Speed
     * UNIT: degrees
     */
    private double U_of_w_;

    private double x;

    private double y;

    public Pyhiscs(double v_deg){
        this.v_deg = v_deg;
        this.v_rad = v_deg * Math.PI/180;
        this.tan_of_v = Math.tan(v_rad);

        if(Math.cos(V_BORDER_RAD - v_rad) >= 0){
            dead_angle_checker_1 = 1;
        }

        double expression = Math.pow(Math.tan((V_BORDER_RAD-v_rad))/L_TO_D_WATER_RATIO, 2);

        if(expression <= 1){
            dead_angle_check_2 = 1;
        }
    }

    public void calculate(){
        s_of_v = calculate_s_of_v();
        S_of_v = calculate_S_of_v();
        w_rad = calculate_W_rad();
        w_deg = calculate_W_deg();
        U_of_w_= calculate_U_of_w();
        x = calculate_x();
        y = calculate_y();
    }

    private double calculate_s_of_v(){
        if(dead_angle_checker_1 + dead_angle_check_2 != 2){
            return 0;
        }
        double cosPart = Math.cos(V_BORDER_RAD - v_rad);
        double tanPart = Math.tan(V_BORDER_RAD - v_rad) / L_TO_D_WATER_RATIO;
        double sqrtPart = Math.sqrt(1 - Math.pow(tanPart, 2));

        return 0.5 * S0 * S0 * cosPart * (1 + sqrtPart);
    }

    private double calculate_S_of_v(){
        if(v_rad < V_BORDER_RAD){
            return s_of_v;
        }
        else{
            return S0 * S0;
        }
    }

    private double calculate_W_rad(){
        if(S_of_v > 0){
            double sinPart = Math.sin(v_rad);
            double cosPart = Math.cos(v_rad);
            return Math.atan(sinPart/(cosPart - S_of_v));
        }
        else{
            return 0;
        }
    }

    private double calculate_W_deg(){
        if(S_of_v > 0){
            double angleInDegrees = Math.toDegrees(w_rad);
            return (angleInDegrees >= 0) ? angleInDegrees : (180 + angleInDegrees);
        }
        else return 0;
    }

    private double calculate_U_of_w(){
        if(S_of_v > 0){
            double denominator  = Math.sqrt(1 + Math.pow(S_of_v, 2) - 2 * S_of_v * Math.cos(v_rad));
            return W_SPEED_WIND * S_of_v / denominator;
        }
        else return 0;
    }

    private double calculate_x(){
        if(S_of_v > 0){
            return U_of_w_ * Math.sin(w_deg * Math.PI/180);
        }
        else return 0;
    }

    private double calculate_y(){
        if(S_of_v > 0){
            return U_of_w_* Math.cos(w_deg * Math.PI/180);
        }
        else return 0;
    }

    public double getX(){
        return this.x;
    }

    public double getY(){
        return this.y;
    }
}
