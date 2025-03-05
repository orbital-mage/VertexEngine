vec4 calcLight(Light light);
vec4 calcDirectionalLight(DirectionalLight light);
vec4 calcPointLight(PointLight light);
vec4 calcSpotLight(SpotLight light);
float calcDiffuse(vec3 lightDir, vec3 normal);
float calcSpecular(vec3 lightDir, vec3 viewDir, vec3 normal);
float calcShadow(DirectionalShadowMap shadowMap, vec3 lightDir, vec3 normal);
float calcShadow(OmnidirectionalShadowMap shadowMap, vec3 lightPos, vec3 normal);

vec4 calcLight(Light light)
{
    vec4 diffuseColor = getDiffuse();
    vec4 specularColor = getSpecular();

    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 lightDir = normalize(light.position - fragPos);

    float diff = calcDiffuse(lightDir, norm);
    float spec = calcSpecular(lightDir, viewDir, norm);

    vec4 ambient = vec4(light.ambient, 1) * diffuseColor;
    vec4 diffuse = vec4(light.diffuse * diff, 1) * diffuseColor;
    vec4 specular = vec4(light.specular * spec, 1) * specularColor;
    
    float shadow = calcShadow(light.shadowMap, light.position, norm);

    return ambient + (1 - shadow) * (diffuse + specular);
}

vec4 calcDirectionalLight(DirectionalLight light)
{
    vec4 diffuseColor = getDiffuse();
    vec4 specularColor = getSpecular();

    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);
    // lightDir uses light's direction
    vec3 lightDir = normalize(-light.direction);

    float diff = calcDiffuse(lightDir, norm);
    float spec = calcSpecular(lightDir, viewDir, norm);

    vec4 ambient = vec4(light.ambient, 1) * diffuseColor;
    vec4 diffuse = vec4(light.diffuse * diff, 1) * diffuseColor;
    vec4 specular = vec4(light.specular * spec, 1) * specularColor;

    float shadow = calcShadow(light.shadowMap, lightDir, norm);

    return ambient + (1 - shadow) * (diffuse + specular);
}

vec4 calcPointLight(PointLight light)
{
    vec4 diffuseColor = getDiffuse();
    vec4 specularColor = getSpecular();

    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 lightDir = normalize(light.position - fragPos);

    float diff = calcDiffuse(lightDir, norm);
    float spec = calcSpecular(lightDir, viewDir, norm);

    // point light attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    vec4 ambient = vec4(light.ambient * attenuation, 1) * diffuseColor;
    vec4 diffuse = vec4(light.diffuse * diff * attenuation, 1) * diffuseColor;
    vec4 specular = vec4(light.specular * spec * attenuation, 1) * specularColor;

    float shadow = calcShadow(light.shadowMap, light.position, norm);
    
    return ambient + (1 - shadow) * (diffuse + specular);
}

vec4 calcSpotLight(SpotLight light)
{
    vec4 diffuseColor = getDiffuse();
    vec4 specularColor = getSpecular();

    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);
    vec3 lightDir = normalize(light.position - fragPos);

    float diff = calcDiffuse(lightDir, norm);
    float spec = calcSpecular(lightDir, viewDir, norm);

    // attenuation
    float distance = length(light.position - fragPos);
    float attenuation = 1 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    // spotlight intensity
    float theta = dot(lightDir, normalize(-light.direction));
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0, 1);

    vec4 ambient = vec4(light.ambient * attenuation, 1) * diffuseColor;
    vec4 diffuse = vec4(light.diffuse * diff * attenuation * intensity, 1) * diffuseColor;
    vec4 specular = vec4(light.specular * spec * attenuation * intensity, 1) * specularColor;

    return ambient + diffuse + specular;
}

float calcDiffuse(vec3 lightDir, vec3 normal)
{
    return max(dot(normal, lightDir), 0);
}

float calcSpecular(vec3 lightDir, vec3 viewDir, vec3 normal)
{
    vec3 halfwayDir = normalize(lightDir + viewDir);
    return pow(max(dot(normal, halfwayDir), 0), shininess);
}

float calcShadow(DirectionalShadowMap shadowMap, vec3 lightDir, vec3 normal)
{
    if (!shadowMap.enable) return 0.0;
    
    vec4 lightSpaceFragPos = vec4(fragPos, 1) * shadowMap.space;
    vec3 projCoords = lightSpaceFragPos.xyz / lightSpaceFragPos.w;
    projCoords = projCoords * 0.5 + 0.5;

    float currentDepth = projCoords.z;
    if (currentDepth > 1) return 0.0;

    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);

    float shadow = 0.0;
    int radius = shadowMap.pcfRadius;
    if (radius > 0) {
        vec2 texelSize = 1.0 / textureSize(shadowMap.map, 0);
        for (int x = -radius; x <= radius; ++x)
        {
            for (int y = -radius; y <= radius; ++y)
            {
                float pcfDepth = texture(shadowMap.map, projCoords.xy + vec2(x, y) * texelSize).r;
                shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;
            }
        }
        shadow /= pow(radius * 2 + 1, 2.0);
    } else {
        float closestDepth = texture(shadowMap.map, projCoords.xy).r;
        shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
    }

    return shadow;
}

float calcShadow(OmnidirectionalShadowMap shadowMap, vec3 lightPos, vec3 normal)
{
    if (!shadowMap.enable) return 0.0;
    
    vec3 fragToLight = fragPos - lightPos;
    vec3 lightDir = normalize(fragToLight);
    
    float currentDepth = length(fragToLight);

    float shadow;
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    
    int samples = shadowMap.pcfSamples;
    float offset = shadowMap.pcfOffset;
    
    if (samples > 0 && offset > 0) {
        float step = offset / samples;
        for (float x = -offset; x < offset; x += step)
        {
            for (float y = -offset; y < offset; y += step)
            {
                for (float z = -offset; z < offset; z += step) {
                    float pcfDepth = texture(shadowMap.map, fragToLight + vec3(x, y, z)).r;
                    pcfDepth *= shadowMap.farPlane;
                    shadow += currentDepth - bias > pcfDepth ? 1.0 : 0.0;
                }
            }
        }
        shadow /= pow(samples * 2, 3.0);
    } else {
        float closestDepth = texture(shadowMap.map, fragToLight).r;
        closestDepth *= shadowMap.farPlane;
        shadow = currentDepth - bias > closestDepth ? 1.0 : 0.0;
    }

    return shadow;
}