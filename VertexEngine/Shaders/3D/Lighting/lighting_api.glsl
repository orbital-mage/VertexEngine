#define LIGHTS 0
#define DIR_LIGHTS 0
#define POINT_LIGHTS 0
#define SPOTLIGHTS 0

#if LIGHTS > 0
uniform Light lights[LIGHTS];
#endif

#if DIR_LIGHTS > 0
uniform DirectionalLight dirLights[DIR_LIGHTS];
#endif

#if POINT_LIGHTS > 0
uniform PointLight pointLights[POINT_LIGHTS];
#endif

#if SPOTLIGHTS > 0
uniform SpotLight spotlights[SPOTLIGHTS];
#endif

vec4 calcLights() 
{
    vec4 result = vec4(0);
    
    #if LIGHTS == 0 && DIR_LIGHTS == 0 && POINT_LIGHTS == 0 && SPOTLIGHTS == 0
    result = getDiffuse();
    #endif
    
    #if LIGHTS > 0
    for (int i = 0; i < LIGHTS; i++)
        result += calcLight(lights[i]);
    #endif

    #if DIR_LIGHTS > 0
    for (int i = 0; i < DIR_LIGHTS; i++)
        result += calcDirectionalLight(dirLights[i]);
    #endif

    #if POINT_LIGHTS > 0
    for (int i = 0; i < POINT_LIGHTS; i++)
        result += calcPointLight(pointLights[i]);
    #endif

    #if SPOTLIGHTS > 0
    for (int i = 0; i < SPOTLIGHTS; i++)
        result += calcSpotLight(spotlights[i]);
    #endif
    
    return result;
}