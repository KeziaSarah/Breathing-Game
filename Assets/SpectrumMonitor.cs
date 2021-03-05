using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumMonitor : MonoBehaviour
{   
    public SimpleSpectrum simpleSpectrum;

    public float threshold;

    public ParticleSystem targetParticleSystem;
    public Color underThresholdColor;
    public Color overThresholdColor;

    public enum Mode
    {
        Average,
        Peak,
    }
    public Mode mode;

    [Range(0f, 100f)]
    public float rangeMinimumPercent = 0f;
    [Range(0f, 100f)]
    public float rangeMaximumPercent = 100f;

    public float currentPeakValue;
    public float currentAverageValue;

    private int _lowerElement;
    private int _upperElement;

    private void Start()
    {
        // get lower and upper elements for ranges as percentage 
        var lower =  simpleSpectrum.barAmount * (rangeMinimumPercent / 100f);
        _lowerElement = (int)lower;

        var upper = simpleSpectrum.barAmount * (rangeMaximumPercent / 100f);
        _upperElement = (int)upper;
    }

    private void Update()
    {       
        switch (mode)
        {
            case Mode.Average:
                ProcessAverageValue();
                currentPeakValue = 0f;
                break;
            case Mode.Peak:
                ProcessPeakValue();
                currentAverageValue = 0f;
                break;        
            default:
                break;
        }
    }

    private void ProcessAverageValue()
    {
        var main = targetParticleSystem.main;

        float sum = 0f;

        var numberOfElements = _upperElement - _lowerElement;

        for (int i = _lowerElement; i < _upperElement; i++)
        {
            sum += simpleSpectrum.spectrumOutputData[i];           
        }

        currentAverageValue = sum / numberOfElements;

        if(currentAverageValue > threshold)
            main.startColor = overThresholdColor;
        else
            main.startColor = underThresholdColor;
    }

    private void ProcessPeakValue()
    {
        var main = targetParticleSystem.main;
        main.startColor = underThresholdColor;

        currentPeakValue = 0f;

        for (int i = _lowerElement; i <_upperElement; i++)
        {
            if (simpleSpectrum.spectrumOutputData[i] > currentPeakValue)
                currentPeakValue = simpleSpectrum.spectrumOutputData[i];

            if (currentPeakValue > threshold)
            {
                main.startColor = overThresholdColor;
            }
        }
    }
}
