using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro; 

public class daynight : MonoBehaviour
{
    //bool to turn on in case the piece is being played in the GO5 space and therefore needs the sound playing. 
    [SerializeField]
    private bool go5;
    private AudioSource cricketSounds;
    bool playing; 

    //most of this code comes from this youtube tutorial: https://www.youtube.com/watch?v=L4t2c1_Szdk , however has been adapted and had aspects added to it eg the skybox, lighting controls and addition of street lights
    //to fit my needs.

    [SerializeField]
    private float timeMultiplier;  //how fast time passes 

    [SerializeField]
    private float startHour;

    [SerializeField]
    private TextMeshProUGUI timeText;
    private string timeBefore;
    private string holder; 
    private string ampm; 

    public DateTime currentTime; 

    [SerializeField]
    private Light sunLight; 

    [SerializeField]
    private float sunriseHour;
    private TimeSpan sunriseTime; 

    [SerializeField]
    private float sunsetHour; 
    private TimeSpan sunsetTime;

    //ambient light colours
    [SerializeField]
    private Color dayAmbientLight;
    [SerializeField]
    private Color nightAmbientLight;

    [SerializeField]
    private AnimationCurve lightChangeCurve; 

    //change the intensity of the light
    [SerializeField]
    private float maxSunLightIntensity; 

    //add moonlight 
    [SerializeField]
    private Light moonLight; 
    [SerializeField]
    private float maxMoonLightIntensity; 

    //change the exposure of the skybox along with the time so it gets dark too
    [SerializeField]
    private Material skybox; 

    public GameObject[] lights; 

    // Start is called before the first frame update
    void Start()
    {     
        cricketSounds = GetComponent<AudioSource>();

        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
        
        //times are dictated in the unity inspector 
        sunriseTime = TimeSpan.FromHours(sunriseHour); 
        sunsetTime = TimeSpan.FromHours(sunsetHour); 
        cricketSounds.Play(); 
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay(); //keep the time moving! 
        RotateSun(); 
        UpdateLightSettings();
        
        if(currentTime.TimeOfDay == TimeSpan.FromHours(12) && go5){
            cricketSounds.Play(); 
        }
    }

    private void UpdateTimeOfDay(){ 
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier); //make the time go up 

        timeBefore = currentTime.ToString("HH.mm");

        if(currentTime.TimeOfDay < TimeSpan.FromHours(12)){
            ampm = " am";
        } else {
            ampm = " pm";
        }

        holder = String.Concat(timeBefore, ampm);
        timeText.text = holder;
    }

    private void RotateSun(){

        float sunLightRotation; 

        if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime){
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0,180, (float)percentage);
            
            //turn the streetlights off in the day 
            foreach(GameObject light in lights){
                light.SetActive(false); 
            }

            if(go5){
                cricketSounds.Stop();
            }

        } else {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);
        
            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180,360, (float)percentage);

            foreach(GameObject light in lights){
                light.SetActive(true); 
            }
        }
        //rotate the sun!! 
        sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime){
        TimeSpan difference = toTime - fromTime; 

        if(difference.TotalSeconds < 0){
            difference += TimeSpan.FromHours(24);
        }

        return difference; 
    }

    private void UpdateLightSettings(){
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);

        //using the animation curve means it doesn't create a non linear transition 
        sunLight.intensity = Mathf.Lerp(-2, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));

        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0.0f, lightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));

        skybox.SetFloat("_Exposure", 0.1f + (sunLight.intensity / 3));
    }
}
