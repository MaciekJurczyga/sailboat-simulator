package org.example;

import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.JFreeChart;
import org.jfree.chart.plot.PlotOrientation;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;

import javax.swing.*;
import java.util.ArrayList;
import java.util.List;

public class Main {
    public static void main(String[] args) {
        List<Double> xs = new ArrayList<>();
        List<Double> ys = new ArrayList<>();

        for(double i = 0; i <= 180; i++){
            Pyhiscs boatPhysics = new Pyhiscs(i);
            boatPhysics.calculate();
            xs.add(boatPhysics.getX());
            ys.add(boatPhysics.getY());
        }


        XYSeries series = new XYSeries("Boat Physics");

        for (int i = 0; i < xs.size(); i++) {
            series.add(xs.get(i), ys.get(i));
        }

        XYSeriesCollection dataset = new XYSeriesCollection(series);


        JFreeChart chart = ChartFactory.createScatterPlot(
                "Boat Physics Chart",
                "X-axis (Angle)",
                "Y-axis (Speed)",
                dataset,
                PlotOrientation.VERTICAL,
                true,
                true,
                false
        );

        ChartPanel chartPanel = new ChartPanel(chart);
        chartPanel.setPreferredSize(new java.awt.Dimension(800, 600));

        JFrame frame = new JFrame();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.getContentPane().add(chartPanel);
        frame.pack();
        frame.setVisible(true);
    }
}
