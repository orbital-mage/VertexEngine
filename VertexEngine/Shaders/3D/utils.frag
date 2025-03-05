in vec2 textureCoords;
in vec3 normal;
in vec3 fragPos;

uniform vec3 viewPos;
uniform samplerCube environment;
uniform float shininess;
uniform vec3 diffuse3;
uniform vec4 diffuse4;
uniform sampler2D diffuseTexture;
uniform vec4 specular4;
uniform vec3 specular3;
uniform sampler2D specularTexture;
uniform float reflection;
uniform vec3 reflection3;
uniform vec4 reflection4;
uniform sampler2D reflectionTexture;
uniform float refraction;
uniform float refractionIndex;
uniform vec3 refraction3;
uniform vec4 refraction4;
uniform sampler2D refractionTexture;
uniform float textureScaling;

out vec4 Output;

vec4 getDiffuse();
vec4 getSpecular();

#include ~/_3D/Lighting/lighting.glsl

float getTextureScaling() 
{
    if (textureScaling == 0)
        return 1.0; 
    else
        return textureScaling;
}

vec2 getTextureCoords() {
    return mod(textureCoords / getTextureScaling(), 1);
}

#if !defined(OVERRIDE_DIFFUSE)
vec4 getDiffuse()
{
    #ifdef DIFFUSE_TEXTURE
    return texture(diffuseTexture, getTextureCoords());
    #elif defined(DIFFUSE_4)
    return diffuse4;
    #elif defined(DIFFUSE_3)
    return vec4(diffuse3, 1);
    #else
    return vec4(1);
    #endif
}
#endif

vec4 getSpecular()
{
    #ifdef SPECULAR_TEXTURE
    return texture(specularTexture, getTextureCoords());
    #elif defined(SPECULAR_4)
    return specular4;
    #elif defined(SPECULAR_3)
    return vec4(specular3, 1);
    #else
    return vec4(0);
    #endif
}

vec4 calcReflection() 
{
    vec4 reflectColor;
    #ifdef REFLECTION_TEXTURE
    reflectColor = texture(reflectionTexture, getTextureCoords());
    #elif defined(REFLECTION_4)
    reflectColor = reflection4;
    #elif defined(RELFECTION_3)
    reflectColor = vec4(reflection3, 1);
    #elif defined(REFLECTION)
    reflectColor = vec4(vec3(reflection), 1);
    #else
    return vec4(0);
    #endif
    
    vec3 viewDir = normalize(fragPos - viewPos);
    vec3 reflectDir = reflect(viewDir, normalize(normal));
    
    return reflectColor * vec4(texture(environment, reflectDir).rgb, 1);
}

vec4 calcRefraction() 
{
    vec4 refractColor;
    #ifdef REFRACTION_TEXTURE
    refractColor = texture(refractionTexture, getTextureCoords());
    #elif defined(REFRACTION_4)
    refractColor = refraction4;
    #elif defined(REFRACTION_3)
    refractColor = vec4(refraction3, 1);
    #elif defined(REFRACTION)
    refractColor = vec4(vec3(refraction), 1);
    #else
    return vec4(0);
    #endif
    
    vec3 viewDir = normalize(fragPos - viewPos);
    vec3 refractDir = refract(viewDir, normalize(normal), 1.0 / refractionIndex);
    
    return refractColor * vec4(texture(environment, refractDir).rgb, 1);
}