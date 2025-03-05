struct DirectionalShadowMap {
    bool enable;
    sampler2D map;
    mat4 space;
    int pcfRadius;
};

struct OmnidirectionalShadowMap {
    bool enable;
    samplerCube map;
    float farPlane;
    float pcfOffset;
    int pcfSamples;
};

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    OmnidirectionalShadowMap shadowMap;
};

struct DirectionalLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    DirectionalShadowMap shadowMap;
};

struct PointLight {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;

    OmnidirectionalShadowMap shadowMap;
};

struct SpotLight {
    vec3 position;
    vec3 direction;

    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};