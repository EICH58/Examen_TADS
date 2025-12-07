# Documentación de Integración Continua

## Flujo de trabajo
El flujo de trabajo automatizado se ejecuta en GitHub Actions y está compuesto por las siguientes etapas:

### 1. **Checkout del código**
- Se descarga el repositorio completo (`fetch-depth: 0`)
- Se ejecuta automáticamente en:
  - Push a las ramas `main` y `develop`
  - Pull requests hacia la rama `main`

### 2. **Configuración del entorno .NET**
- Se establece .NET SDK versión 9.0.308
- Se prepara el entorno para compilación y pruebas

### 3. **Instalación de herramientas de análisis**
- Se instala `dotnet-sonarscanner` globalmente
- Se inicia el análisis de SonarCloud con parámetros configurados

### 4. **Compilación**
- Se compila el proyecto con la configuración `Release`
- Se detectan warnings y errores durante la compilación

### 5. **Ejecución de pruebas con cobertura**
- **Primer intento**: Ejecución de pruebas con XPlat Code Coverage
- **Fallback**: Si no se genera cobertura, se usa Coverlet
- Se busca automáticamente los archivos de cobertura generados

### 6. **Conversión de formato de cobertura**
- Se convierte el reporte de cobertura a formato OpenCover
- Se usa `reportgenerator-globaltool` para la conversión
- Se manejan errores si no se encuentra ningún reporte

### 7. **Finalización del análisis SonarCloud**
- Se envía la información de cobertura y análisis a SonarCloud
- Se cierra la sesión del scanner

## Análisis de calidad de código
El flujo incluye integración con SonarCloud para análisis estático que detecta:

### **Problemas identificados**
- **1 Warning**: CS8603 - Posible retorno de referencia nula en `Alumno.cs` (línea 94)
- **0 Errores** de compilación
- **Pruebas ejecutadas exitosamente**

### **Métricas analizadas por SonarCloud**
- **Bugs** potenciales
- **Vulnerabilidades** de seguridad
- **Code smells** (malas prácticas)
- **Cobertura de código** (con conversión a OpenCover)
- **Duplicación** de código
- **Deuda técnica**

## Resultados
Los resultados de la integración continua se pueden ver en:

### **GitHub Actions**
- **Pestaña "Actions"** del repositorio GitHub: [Enlace al workflow](https://github.com/EICH58/Examen_TADS/actions)
- **Tiempo de ejecución**: ~25 segundos
- **Estado**: Éxito en todas las ejecuciones recientes

### **SonarCloud**
- **Dashboard de calidad**: [https://sonarcloud.io/project/overview?id=EICH58_Examen_TADS](https://sonarcloud.io/project/overview?id=EICH58_Examen_TADS)
- **Reportes detallados**: Bugs, vulnerabilidades, code smells
- **Cobertura de pruebas**: Generada automáticamente con conversión a OpenCover

### **Artifacts generados**
- **Binarios compilados**: En configuración Release
- **Reportes de cobertura**: En formatos cobertura y OpenCover
- **Resultados de pruebas**: Ejecutadas en cada run

## Requisitos técnicos

### **Entorno de ejecución**
- **Sistema operativo**: Ubuntu 24.04 LTS (última versión)
- **.NET SDK**: 9.0.308
- **Git**: 2.52.0+

### **Herramientas instaladas**
- `dotnet-sonarscanner` - Para análisis con SonarCloud
- `reportgenerator-globaltool` - Para conversión de reportes de cobertura
- `dotnet test` - Con soporte para XPlat Code Coverage

### **Secrets necesarios**
- `SONAR_TOKEN`: Token de autenticación para SonarCloud
  - Configurado en GitHub Secrets
  - Usado para enviar análisis a SonarCloud

### **Configuración del proyecto**
- **Target framework**: net8.0 (verificar consistencia con SDK 9.0.308)
- **Configuración de compilación**: Release
- **Exclusiones de cobertura**: Archivos que contienen "Test" en el nombre

## Configuración del repositorio

### **Estructura esperada**
```
.github/workflows/
└── dotnet-ci.yml          # Archivo de workflow
src/
└── SistemaRegistroAlumnos/
    ├── SistemaRegistroAlumnos.csproj
    ├── Models/
    │   └── Alumno.cs      # Contiene warning CS8603
    └── Tests/             # Proyectos de prueba
coverage/                  # Directorio generado para reportes
```

### **Branches configuradas**
- **main**: Rama principal protegida
- **develop**: Rama de desarrollo
- **Otras ramas**: feature/, fix/, etc.

## Solución de problemas

### **Problemas comunes**
1. **Warning CS8603**: Retorno posiblemente nulo
   - Solución: Añadir validación nula o usar operadores null-conditional

2. **Falta de cobertura generada**
   - El workflow tiene mecanismos de fallback automáticos
   - Verifica que los proyectos de prueba estén correctamente configurados

3. **Error de autenticación con SonarCloud**
   - Verifica que el secret `SONAR_TOKEN` esté configurado en GitHub
   - Confirma que el token tenga permisos adecuados

### **Monitorización**
- Revisar regularmente el dashboard de SonarCloud
- Corregir issues críticos y bloquear merges si hay problemas graves
- Monitorear el tiempo de ejecución del workflow

## Mejoras futuras recomendadas

### **Inmediatas**
1. Corregir el warning CS8603 en `Alumno.cs`
2. Verificar consistencia entre target framework (net8.0) y SDK (9.0.308)
3. Añadir cache de dependencias NuGet para acelerar builds

### **A medio plazo**
1. Implementar análisis de seguridad de dependencias
2. Añadir steps para publicación de artefactos
3. Configurar notificaciones de estado de CI

### **Avanzadas**
1. Implementar canary deployments
2. Añadir análisis de rendimiento
3. Integrar con herramientas de revisión de código

---

**Última ejecución exitosa**: 17 horas ago  
**Tiempo promedio**: 25 segundos  
**Estado actual**: Funcionando correctamente
