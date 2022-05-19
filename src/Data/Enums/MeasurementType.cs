using System.ComponentModel;

namespace Data.Enums;

public enum MeasurementType
{
    [Description("battery pct")]
    battery_pct,
    [Description("battery level")]
    battery_level,
    [Description("power on")]
    power_on,
    [Description("reactive power var")]
    reactive_power_var,
    [Description("active power w")]
    active_power_w,
    [Description("reactive energy varh")]
    reactive_energy_varh,
    [Description("active energy wh")]
    active_energy_wh,
    [Description("ampere amp")]
    ampere_amp,
    [Description("temperature air c")]
    temperature_c,
    [Description("temperature road c")]
    temperature_road_c,
    [Description("temperature ground c")]
    temperature_ground_c,
    [Description("temperature head c")]
    temperature_head_c,
    [Description("humidity pct")]
    humidity_pct,
    [Description("co2 ppm")]
    co2_ppm,
    [Description("dewpoint c")]
    dewpoint_c,
    [Description("pressure hpa")]
    pressure_hpa,
    [Description("count")]
    count,
    [Description("count acc")]
    count_acc,
    [Description("count samples")]
    count_samples,
    [Description("count pm 10")]
    count_pm10,
    [Description("count pm 4")]
    count_pm4,
    [Description("count pm 2.5")]
    count_pm2_5,
    [Description("count pm 0.5")]
    count_pm0_5,
    [Description("count pm 1")]
    count_pm1,
    [Description("mass microgram pm 1")]
    mass_microgram_pm1,
    [Description("mass microgram pm 2.5")]
    mass_microgram_pm2_5,
    [Description("mass microgram pm 4")]
    mass_microgram_pm4,
    [Description("mass microgram pm 10")]
    mass_microgram_pm10,
    [Description("particlesize nm")]
    particlesize_nm,
    [Description("distance cm")]
    distance_cm,
    [Description("lux lumen")]
    lux_lumen,
    [Description("sound avg db")]
    sound_avg_db,
    [Description("sound peak db")]
    sound_peak_db,
    [Description("sound current db")]
    sound_current_db,
    [Description("sound low db")]
    sound_low_db,
    [Description("light color")]
    light_color,
    [Description("light level")]
    light_level,
    [Description("movement")]
    movement,
    [Description("occupancy")]
    occupancy,
    [Description("rssi_dbm")]
    rssi_dbm,
    [Description("voc_ppb")]
    voc_ppb,
    [Description("voltage v")]
    voltage_v,
    [Description("door open")]
    door_open,
    [Description("door close")]
    door_close,
    [Description("count dm1 activity acc")]
    count_dm1_activity_acc,
    [Description("count dm2 activity acc")]
    count_dm2_activity_acc,
    [Description("count dm1 tick")]
    count_dm1_tick,
    [Description("count dm2 tick")]
    count_dm2_tick,
    [Description("count dm3 tick")]
    count_dm3_tick,
    [Description("count dm4 tick")]
    count_dm4_tick,
}
