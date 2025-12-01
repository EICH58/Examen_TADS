// =========================================
// MENÚ DE ACCESIBILIDAD - JavaScript
// Sistema completo de accesibilidad web
// =========================================

document.addEventListener('DOMContentLoaded', function() {
    
    // ==================== REFERENCIAS A ELEMENTOS ====================
    const toggleBtn = document.getElementById('accessibilityToggle');
    const panel = document.getElementById('accessibilityPanel');
    const closeBtn = document.getElementById('closeAccessibility');
    const resetBtn = document.getElementById('resetAccessibility');

    // ==================== GESTIÓN DEL PANEL ====================
    
    // Abrir/cerrar panel
    if (toggleBtn) {
        toggleBtn.addEventListener('click', function(e) {
            e.stopPropagation();
            panel.classList.toggle('active');
        });
    }

    if (closeBtn) {
        closeBtn.addEventListener('click', function() {
            panel.classList.remove('active');
        });
    }

    // Cerrar al hacer clic fuera
    document.addEventListener('click', function(event) {
        const menu = document.querySelector('.accessibility-menu');
        if (menu && !menu.contains(event.target)) {
            panel.classList.remove('active');
        }
    });

    // Prevenir cierre al hacer clic dentro del panel
    if (panel) {
        panel.addEventListener('click', function(event) {
            event.stopPropagation();
        });
    }

    // ==================== DECLARACIÓN DE VARIABLES ====================
    // Declarar todas las variables primero para evitar errores de inicialización
    let currentZoom = 100; // Variable para el zoom
    
    const highContrast = document.getElementById('highContrast');
    const textBtns = document.querySelectorAll('[id^="text"]');
    const fontFamily = document.getElementById('fontFamily');
    const zoomIn = document.getElementById('zoomIn');
    const zoomOut = document.getElementById('zoomOut');
    const zoomNormal = document.getElementById('zoomNormal');
    const invertColors = document.getElementById('invertColors');
    const screenReader = document.getElementById('screenReader');
    const visualIndicators = document.getElementById('visualIndicators');
    const readingGuide = document.getElementById('readingGuide');
    const colorBlindMode = document.getElementById('colorBlindMode');
    const onScreenKeyboard = document.getElementById('onScreenKeyboard');
    const bigCursor = document.getElementById('bigCursor');
    const highlightLinks = document.getElementById('highlightLinks');
    const readAloud = document.getElementById('readAloud');
    const focusMode = document.getElementById('focusMode');
    const simplifiedMenus = document.getElementById('simplifiedMenus');

    // ==================== CARGAR CONFIGURACIÓN GUARDADA ====================
    cargarConfiguracion();

    // ==================== 1. VISUALES ====================

    if (highContrast) {
        highContrast.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('high-contrast');
            } else {
                document.body.classList.remove('high-contrast');
            }
            guardarConfiguracion('highContrast', this.checked);
        });
    }

    // Tamaño de texto
    textBtns.forEach(btn => {
        btn.addEventListener('click', function() {
            textBtns.forEach(b => b.classList.remove('active'));
            this.classList.add('active');
            
            const size = this.id.replace('text', '').toLowerCase();
            document.body.classList.remove('text-small', 'text-normal', 'text-large');
            
            if (size === 'small') {
                document.body.classList.add('text-small');
            } else if (size === 'large') {
                document.body.classList.add('text-large');
            }
            
            guardarConfiguracion('textSize', size);
        });
    });

    // Tipo de letra
    if (fontFamily) {
        fontFamily.addEventListener('change', function() {
            const font = this.value;
            if (font === 'default') {
                document.body.style.fontFamily = '';
            } else {
                document.body.style.fontFamily = font;
            }
            guardarConfiguracion('fontFamily', font);
        });
    }

    // Zoom de pantalla
    if (zoomIn) {
        zoomIn.addEventListener('click', function() {
            currentZoom = Math.min(currentZoom + 10, 150);
            aplicarZoom(currentZoom);
        });
    }

    if (zoomOut) {
        zoomOut.addEventListener('click', function() {
            currentZoom = Math.max(currentZoom - 10, 80);
            aplicarZoom(currentZoom);
        });
    }

    if (zoomNormal) {
        zoomNormal.addEventListener('click', function() {
            currentZoom = 100;
            aplicarZoom(100);
        });
    }

    function aplicarZoom(zoom) {
        document.body.style.zoom = zoom + '%';
        document.querySelectorAll('[id^="zoom"]').forEach(btn => btn.classList.remove('active'));
        if (zoom === 100) zoomNormal.classList.add('active');
        guardarConfiguracion('zoom', zoom);
    }

    // Inversión de colores
    if (invertColors) {
        invertColors.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('invert-colors');
            } else {
                document.body.classList.remove('invert-colors');
            }
            guardarConfiguracion('invertColors', this.checked);
        });
    }

    // Lector de pantalla
    if (screenReader) {
        screenReader.addEventListener('change', function() {
            if (this.checked) {
                activarLectorPantalla();
            } else {
                desactivarLectorPantalla();
            }
            guardarConfiguracion('screenReader', this.checked);
        });
    }

    // Indicadores visuales
    if (visualIndicators) {
        visualIndicators.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('visual-indicators');
            } else {
                document.body.classList.remove('visual-indicators');
            }
            guardarConfiguracion('visualIndicators', this.checked);
        });
    }

    // Guía de lectura
    if (readingGuide) {
        readingGuide.addEventListener('change', function() {
            if (this.checked) {
                activarGuiaLectura();
            } else {
                desactivarGuiaLectura();
            }
            guardarConfiguracion('readingGuide', this.checked);
        });
    }

    // Modo daltonismo
    if (colorBlindMode) {
        colorBlindMode.addEventListener('change', function() {
            const mode = this.value;
            
            // Quitar todas las clases de daltonismo
            document.body.classList.remove(
                'colorblind-protanopia',
                'colorblind-deuteranopia', 
                'colorblind-tritanopia',
                'colorblind-achromatopsia'
            );
            
            // Agregar la clase correspondiente
            if (mode !== 'none') {
                document.body.classList.add('colorblind-' + mode);
            }
            
            guardarConfiguracion('colorBlindMode', mode);
        });
    }

    // ==================== 3. MOTORAS Y FÍSICAS ====================

    // Teclado en pantalla
    if (onScreenKeyboard) {
        onScreenKeyboard.addEventListener('change', function() {
            if (this.checked) {
                mostrarTecladoPantalla();
            } else {
                ocultarTecladoPantalla();
            }
            guardarConfiguracion('onScreenKeyboard', this.checked);
        });
    }

    // Inicializar funcionalidad del teclado
    inicializarTecladoPantalla();

    // Puntero grande
    if (bigCursor) {
        bigCursor.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('big-cursor');
            } else {
                document.body.classList.remove('big-cursor');
            }
            guardarConfiguracion('bigCursor', this.checked);
        });
    }

    // Resaltar enlaces
    if (highlightLinks) {
        highlightLinks.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('highlight-links');
            } else {
                document.body.classList.remove('highlight-links');
            }
            guardarConfiguracion('highlightLinks', this.checked);
        });
    }

    // ==================== 4. COGNITIVAS ====================

    // Lectura en voz alta
    if (readAloud) {
        readAloud.addEventListener('change', function() {
            if (this.checked) {
                activarLecturaVozAlta();
            } else {
                desactivarLecturaVozAlta();
            }
            guardarConfiguracion('readAloud', this.checked);
        });
    }

    // Modo enfoque
    if (focusMode) {
        focusMode.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('focus-mode');
            } else {
                document.body.classList.remove('focus-mode');
            }
            guardarConfiguracion('focusMode', this.checked);
        });
    }

    // Menús simplificados
    if (simplifiedMenus) {
        simplifiedMenus.addEventListener('change', function() {
            if (this.checked) {
                document.body.classList.add('simplified-menus');
            } else {
                document.body.classList.remove('simplified-menus');
            }
            guardarConfiguracion('simplifiedMenus', this.checked);
        });
    }

    // ==================== BOTÓN RESETEAR ====================
    if (resetBtn) {
        resetBtn.addEventListener('click', function() {
            if (confirm('¿Estás seguro de restablecer todas las configuraciones de accesibilidad?')) {
                resetearConfiguracion();
            }
        });
    }

    // ==================== FUNCIONES AUXILIARES ====================

    // Guardar configuración en localStorage
    function guardarConfiguracion(key, value) {
        localStorage.setItem('accessibility_' + key, JSON.stringify(value));
    }

    // Cargar configuración guardada
    function cargarConfiguracion() {
        // Alto contraste
        const savedHighContrast = JSON.parse(localStorage.getItem('accessibility_highContrast'));
        if (savedHighContrast && highContrast) {
            highContrast.checked = true;
            document.body.classList.add('high-contrast');
        }

        // Tamaño de texto
        const savedTextSize = localStorage.getItem('accessibility_textSize');
        if (savedTextSize) {
            const parsedSize = JSON.parse(savedTextSize);
            document.body.classList.add('text-' + parsedSize);
            textBtns.forEach(btn => {
                if (btn.id === 'text' + parsedSize.charAt(0).toUpperCase() + parsedSize.slice(1)) {
                    btn.classList.add('active');
                } else {
                    btn.classList.remove('active');
                }
            });
        }

        // Cargar otras configuraciones...
        cargarOtrasConfiguraciones();
    }

    function cargarOtrasConfiguraciones() {
        // Tipo de letra
        const savedFont = JSON.parse(localStorage.getItem('accessibility_fontFamily'));
        if (savedFont && fontFamily) {
            fontFamily.value = savedFont;
            if (savedFont !== 'default') {
                document.body.style.fontFamily = savedFont;
            }
        }

        // Zoom
        const savedZoom = JSON.parse(localStorage.getItem('accessibility_zoom'));
        if (savedZoom) {
            currentZoom = savedZoom;
            aplicarZoom(savedZoom);
        }

        // Inversión de colores
        const savedInvert = JSON.parse(localStorage.getItem('accessibility_invertColors'));
        if (savedInvert && invertColors) {
            invertColors.checked = true;
            document.body.classList.add('invert-colors');
        }

        // Puntero grande
        const savedBigCursor = JSON.parse(localStorage.getItem('accessibility_bigCursor'));
        if (savedBigCursor && bigCursor) {
            bigCursor.checked = true;
            document.body.classList.add('big-cursor');
        }

        // Resaltar enlaces
        const savedHighlightLinks = JSON.parse(localStorage.getItem('accessibility_highlightLinks'));
        if (savedHighlightLinks && highlightLinks) {
            highlightLinks.checked = true;
            document.body.classList.add('highlight-links');
        }

        // Lectura en voz alta
        const savedReadAloud = JSON.parse(localStorage.getItem('accessibility_readAloud'));
        if (savedReadAloud && readAloud) {
            readAloud.checked = true;
            activarLecturaVozAlta();
        }

        // Modo enfoque
        const savedFocusMode = JSON.parse(localStorage.getItem('accessibility_focusMode'));
        if (savedFocusMode && focusMode) {
            focusMode.checked = true;
            document.body.classList.add('focus-mode');
        }

        // Menús simplificados
        const savedSimplified = JSON.parse(localStorage.getItem('accessibility_simplifiedMenus'));
        if (savedSimplified && simplifiedMenus) {
            simplifiedMenus.checked = true;
            document.body.classList.add('simplified-menus');
        }

        // Indicadores visuales
        const savedVisual = JSON.parse(localStorage.getItem('accessibility_visualIndicators'));
        if (savedVisual && visualIndicators) {
            visualIndicators.checked = true;
            document.body.classList.add('visual-indicators');
        }

        // Lector de pantalla
        const savedScreenReader = JSON.parse(localStorage.getItem('accessibility_screenReader'));
        if (savedScreenReader && screenReader) {
            screenReader.checked = true;
            activarLectorPantalla();
        }

        // Teclado en pantalla
        const savedKeyboard = JSON.parse(localStorage.getItem('accessibility_onScreenKeyboard'));
        if (savedKeyboard && onScreenKeyboard) {
            onScreenKeyboard.checked = true;
            mostrarTecladoPantalla();
        }

        // Guía de lectura
        const savedReadingGuide = JSON.parse(localStorage.getItem('accessibility_readingGuide'));
        if (savedReadingGuide && readingGuide) {
            readingGuide.checked = true;
            activarGuiaLectura();
        }

        // Modo daltonismo
        const savedColorBlind = JSON.parse(localStorage.getItem('accessibility_colorBlindMode'));
        if (savedColorBlind && colorBlindMode) {
            colorBlindMode.value = savedColorBlind;
            if (savedColorBlind !== 'none') {
                document.body.classList.add('colorblind-' + savedColorBlind);
            }
        }
    }

    // Resetear configuración
    function resetearConfiguracion() {
        // Limpiar localStorage
        Object.keys(localStorage).forEach(key => {
            if (key.startsWith('accessibility_')) {
                localStorage.removeItem(key);
            }
        });

        // Recargar página
        location.reload();
    }

    // ==================== FUNCIONES DE GUÍA DE LECTURA ====================

    function activarGuiaLectura() {
        const guideContainer = document.getElementById('readingGuideLines');
        const horizontalLine = guideContainer.querySelector('.reading-guide-horizontal');
        const verticalLine = guideContainer.querySelector('.reading-guide-vertical');
        
        guideContainer.style.display = 'block';
        
        // Seguir el mouse
        document.addEventListener('mousemove', moverGuiaLectura);
        
        function moverGuiaLectura(e) {
            if (horizontalLine && verticalLine) {
                horizontalLine.style.top = e.clientY + 'px';
                verticalLine.style.left = e.clientX + 'px';
            }
        }
        
        // Guardar referencia para poder desactivar
        document.moverGuiaLectura = moverGuiaLectura;
        
        mostrarNotificacion('📏 Guía de lectura activada', 'Una línea seguirá tu cursor para ayudarte a leer');
    }

    function desactivarGuiaLectura() {
        const guideContainer = document.getElementById('readingGuideLines');
        guideContainer.style.display = 'none';
        
        // Remover event listener
        if (document.moverGuiaLectura) {
            document.removeEventListener('mousemove', document.moverGuiaLectura);
            delete document.moverGuiaLectura;
        }
    }

    // ==================== FUNCIONES ESPECÍFICAS ====================

    function mostrarNotificacion(titulo, mensaje) {
        // Crear notificación visual temporal
        const notif = document.createElement('div');
        notif.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            background: #1B396B;
            color: white;
            padding: 15px 20px;
            border-radius: 8px;
            box-shadow: 0 4px 12px rgba(0,0,0,0.3);
            z-index: 10000;
            max-width: 300px;
            animation: slideIn 0.3s ease;
        `;
        notif.innerHTML = `<strong>${titulo}</strong><br>${mensaje}`;
        document.body.appendChild(notif);

        setTimeout(() => {
            notif.style.animation = 'slideOut 0.3s ease';
            setTimeout(() => notif.remove(), 300);
        }, 4000);
    }

    function activarLectorPantalla() {
        console.log('🔊 Lector de pantalla activado');
        // Implementación básica: agregar atributos ARIA y leer elementos al hacer hover
        document.body.setAttribute('data-screen-reader', 'active');
    }

    function desactivarLectorPantalla() {
        console.log('🔇 Lector de pantalla desactivado');
        document.body.removeAttribute('data-screen-reader');
    }

    function activarTranscripcion() {
        console.log('📝 Transcripción activada');
        // Aquí se integraría Web Speech API
    }

    function desactivarTranscripcion() {
        console.log('📝 Transcripción desactivada');
    }

    function activarControlPorVoz() {
        console.log('🎤 Control por voz activado');
        // Aquí se integraría Web Speech API para reconocimiento de voz
    }

    function desactivarControlPorVoz() {
        console.log('🎤 Control por voz desactivado');
    }

    function activarGestos() {
        console.log('👆 Reconocimiento de gestos activado');
        // Implementación de gestos táctiles o con cámara
    }

    function desactivarGestos() {
        console.log('👆 Reconocimiento de gestos desactivado');
    }

    function mostrarTecladoPantalla() {
        console.log('⌨️ Teclado en pantalla mostrado');
        const keyboard = document.getElementById('onScreenKeyboardContainer');
        if (keyboard) {
            keyboard.style.display = 'block';
        }
    }

    function ocultarTecladoPantalla() {
        console.log('⌨️ Teclado en pantalla ocultado');
        const keyboard = document.getElementById('onScreenKeyboardContainer');
        if (keyboard) {
            keyboard.style.display = 'none';
        }
    }

    function inicializarTecladoPantalla() {
        const keyboard = document.getElementById('onScreenKeyboardContainer');
        if (!keyboard) return;

        let activeInput = null;
        let shiftActive = false;

        // Detectar campo de entrada activo
        document.addEventListener('focusin', function(e) {
            if (e.target.matches('input[type="text"], input[type="email"], input[type="password"], input[type="search"], textarea')) {
                activeInput = e.target;
            }
        });

        // Botón cerrar
        const closeBtn = document.getElementById('keyboardClose');
        if (closeBtn) {
            closeBtn.addEventListener('click', function() {
                ocultarTecladoPantalla();
                const checkbox = document.getElementById('onScreenKeyboard');
                if (checkbox) checkbox.checked = false;
            });
        }

        // Tecla Shift
        const shiftKey = document.getElementById('shiftKey');
        if (shiftKey) {
            shiftKey.addEventListener('click', function() {
                shiftActive = !shiftActive;
                this.classList.toggle('active', shiftActive);
            });
        }

        // Todas las teclas
        const keys = keyboard.querySelectorAll('.key');
        keys.forEach(key => {
            key.addEventListener('click', function() {
                if (!activeInput) {
                    alert('Por favor, haz clic en un campo de texto primero');
                    return;
                }

                const keyText = this.textContent.trim();

                if (keyText === '⌫') {
                    // Backspace
                    const cursorPos = activeInput.selectionStart;
                    if (cursorPos > 0) {
                        activeInput.value = activeInput.value.substring(0, cursorPos - 1) + activeInput.value.substring(cursorPos);
                        activeInput.setSelectionRange(cursorPos - 1, cursorPos - 1);
                    }
                } else if (keyText === '↵') {
                    // Enter
                    const event = new KeyboardEvent('keypress', { key: 'Enter' });
                    activeInput.dispatchEvent(event);
                    if (activeInput.form) {
                        activeInput.form.dispatchEvent(new Event('submit', { bubbles: true }));
                    }
                } else if (keyText === 'Espacio') {
                    // Espacio
                    insertarTexto(activeInput, ' ');
                } else if (keyText === '⇧') {
                    // Shift - ya se maneja arriba
                    return;
                } else {
                    // Letras y números
                    let texto = shiftActive ? keyText.toUpperCase() : keyText.toLowerCase();
                    insertarTexto(activeInput, texto);
                    
                    // Desactivar shift después de una tecla
                    if (shiftActive) {
                        shiftActive = false;
                        if (shiftKey) shiftKey.classList.remove('active');
                    }
                }

                // Disparar evento input para que funcionen validaciones
                activeInput.dispatchEvent(new Event('input', { bubbles: true }));
            });
        });

        function insertarTexto(input, texto) {
            const cursorPos = input.selectionStart;
            input.value = input.value.substring(0, cursorPos) + texto + input.value.substring(cursorPos);
            input.setSelectionRange(cursorPos + texto.length, cursorPos + texto.length);
            input.focus();
        }
    }

    function activarFiltradoTeclas() {
        console.log('⌨️ Filtrado de teclas activado');
        // Implementar delay en pulsaciones
    }

    function desactivarFiltradoTeclas() {
        console.log('⌨️ Filtrado de teclas desactivado');
    }

    function activarLecturaVozAlta() {
        console.log('🔊 Lectura en voz alta activada');
        
        // Verificar soporte
        if (!('speechSynthesis' in window)) {
            alert('❌ Tu navegador no soporta síntesis de voz.\n\nPrueba con:\n• Google Chrome\n• Microsoft Edge\n• Safari');
            const checkbox = document.getElementById('readAloud');
            if (checkbox) checkbox.checked = false;
            return;
        }

        document.body.setAttribute('data-read-aloud', 'active');
        
        // Agregar event listener de click global
        document.addEventListener('click', manejarClickLectura);
        
        mostrarNotificacion('🔊 Lectura activada', 'Haz clic en cualquier texto, campo o elemento para escucharlo');
    }

    function desactivarLecturaVozAlta() {
        console.log('🔇 Lectura en voz alta desactivada');
        document.body.removeAttribute('data-read-aloud');
        document.removeEventListener('click', manejarClickLectura);
        
        // Detener cualquier lectura en curso
        if ('speechSynthesis' in window) {
            speechSynthesis.cancel();
        }
    }

    function manejarClickLectura(e) {
        // No leer el menú de accesibilidad
        if (e.target.closest('.accessibility-menu')) {
            return;
        }

        let texto = '';
        let elemento = e.target;
        
        // 1. INPUTS Y TEXTAREAS - Leer label + placeholder + valor
        if (elemento.matches('input, textarea')) {
            // Buscar label asociado
            const label = document.querySelector(`label[for="${elemento.id}"]`) || 
                         elemento.closest('label') ||
                         elemento.previousElementSibling;
            
            if (label && label.tagName === 'LABEL') {
                texto += label.textContent.trim() + '. ';
            }
            
            // Placeholder
            if (elemento.placeholder) {
                texto += elemento.placeholder + '. ';
            }
            
            // Valor actual
            if (elemento.value && elemento.value.trim()) {
                texto += 'Valor actual: ' + elemento.value;
            } else {
                texto += 'Campo vacío';
            }
        }
        // 1B. SELECT (DROPDOWNS) - Leer label + opción seleccionada
        else if (elemento.matches('select')) {
            // Buscar label asociado
            const label = document.querySelector(`label[for="${elemento.id}"]`) || 
                         elemento.closest('label') ||
                         elemento.previousElementSibling;
            
            if (label && label.tagName === 'LABEL') {
                texto += label.textContent.trim() + '. ';
            }
            
            // Leer la opción seleccionada (el TEXTO, no el value)
            const opcionSeleccionada = elemento.options[elemento.selectedIndex];
            if (opcionSeleccionada) {
                texto += 'Opción seleccionada: ' + opcionSeleccionada.textContent.trim();
            } else {
                texto += 'Sin opción seleccionada';
            }
        }
        // 1C. OPCIONES DENTRO DE SELECT
        else if (elemento.matches('option')) {
            const select = elemento.closest('select');
            
            // Buscar label del select
            if (select) {
                const label = document.querySelector(`label[for="${select.id}"]`) || 
                             select.closest('label') ||
                             select.previousElementSibling;
                
                if (label && label.tagName === 'LABEL') {
                    texto += label.textContent.trim() + '. ';
                }
            }
            
            // Leer el texto de la opción
            texto += 'Opción: ' + elemento.textContent.trim();
        }
        // 2. CELDAS DE TABLA - Leer encabezado + contenido
        else if (elemento.matches('td, th')) {
            const tabla = elemento.closest('table');
            const fila = elemento.closest('tr');
            const indiceColumna = Array.from(fila.children).indexOf(elemento);
            
            // Buscar encabezado de la columna
            const thead = tabla.querySelector('thead');
            if (thead) {
                const encabezados = thead.querySelectorAll('th');
                if (encabezados[indiceColumna]) {
                    texto += encabezados[indiceColumna].textContent.trim() + ': ';
                }
            }
            
            // Contenido de la celda
            texto += elemento.textContent.trim();
        }
        // 3. ELEMENTOS SVG/GRÁFICAS - Leer title, aria-label o texto
        else if (elemento.closest('svg') || elemento.tagName === 'svg') {
            const svg = elemento.closest('svg') || elemento;
            
            // Buscar title dentro del SVG
            const title = svg.querySelector('title');
            if (title) {
                texto = title.textContent.trim();
            }
            // O aria-label
            else if (svg.getAttribute('aria-label')) {
                texto = svg.getAttribute('aria-label');
            }
            // O texto del elemento clickeado
            else if (elemento.textContent && elemento.textContent.trim()) {
                texto = elemento.textContent.trim();
            }
            // Fallback
            else {
                texto = 'Gráfica o elemento visual';
            }
        }
        // 4. BOTONES E ICONOS - Leer aria-label, title o texto
        else if (elemento.matches('button, a, i, svg')) {
            texto = elemento.getAttribute('aria-label') || 
                   elemento.getAttribute('title') || 
                   elemento.textContent.trim() ||
                   'Botón o enlace';
        }
        // 5. TEXTO GENERAL - Párrafos, títulos, listas, etc.
        else if (elemento.matches('p, h1, h2, h3, h4, h5, h6, li, span, div, label')) {
            texto = elemento.textContent.trim();
        }
        
        // Leer el texto si existe
        if (texto && texto.length > 0) {
            leerTexto(texto);
            
            // Resaltar elemento mientras se lee
            const originalBg = elemento.style.backgroundColor;
            elemento.style.backgroundColor = 'rgba(100, 200, 255, 0.3)';
            elemento.style.transition = 'background-color 0.2s';
            
            setTimeout(() => {
                elemento.style.backgroundColor = originalBg;
            }, 2000);
        }
    }

    function leerTexto(texto) {
        if (!('speechSynthesis' in window)) return;
        
        // Cancelar lectura anterior
        speechSynthesis.cancel();
        
        // Crear nueva utterance
        const utterance = new SpeechSynthesisUtterance(texto);
        utterance.lang = 'es-MX'; // Español de México
        utterance.rate = 0.9;  // Velocidad (0.9 = más lento para claridad)
        utterance.pitch = 1.0; // Tono normal
        utterance.volume = 1.0; // Volumen máximo
        
        // Seleccionar voz en español si está disponible
        const voices = speechSynthesis.getVoices();
        const spanishVoice = voices.find(voice => voice.lang.startsWith('es'));
        if (spanishVoice) {
            utterance.voice = spanishVoice;
        }
        
        // Hablar
        speechSynthesis.speak(utterance);
    }

    function desactivarLecturaVozAlta() {
        console.log('🔇 Lectura en voz alta desactivada');
        document.body.removeAttribute('data-read-aloud');
        document.removeEventListener('click', manejarClickLectura);
        
        // Detener cualquier lectura en curso
        if ('speechSynthesis' in window) {
            speechSynthesis.cancel();
        }
    }

    console.log('✅ Menú de accesibilidad cargado correctamente');
});
