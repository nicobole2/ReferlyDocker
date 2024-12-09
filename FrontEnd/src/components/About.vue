<script setup>
</script>

<template>
  <main class="about">
    <section class="hero">
      <div class="container">
        <h1>Sobre Nosotros</h1>
        <p class="subtitle">La primera plataforma que te permite ganar dinero refiriendo talento</p>
      </div>
    </section>

    <section class="features">
      <div class="container">
        <div class="feature-grid">
          <div class="feature-card">
            <div class="icon">💰</div>
            <h3>Gana en Dólares</h3>
            <p>Recibe pagos en dólares por cada referido exitoso que consigas</p>
          </div>

          <div class="feature-card">
            <div class="icon">🚀</div>
            <h3>Simple y Rápido</h3>
            <p>Proceso sencillo de referidos: comparte, conecta y gana</p>
          </div>

          <div class="feature-card">
            <div class="icon">🤝</div>
            <h3>Sin Límites</h3>
            <p>Refiere tantos candidatos como quieras, sin restricciones</p>
          </div>

          <div class="feature-card">
            <div class="icon">🌐</div>
            <h3>Alcance Global</h3>
            <p>Conecta talento con empresas de todo el mundo</p>
          </div>
        </div>

        <div class="how-it-works">
          <h2>¿Cómo Funciona?</h2>
          <div class="steps">
            <div class="step">
              <div class="step-number">1</div>
              <h4>Encuentra Oportunidades</h4>
              <p>Explora nuestra base de empleos actualizada diariamente</p>
            </div>
            <div class="step">
              <div class="step-number">2</div>
              <h4>Refiere Candidatos</h4>
              <p>Comparte las ofertas con candidatos calificados</p>
            </div>
            <div class="step">
              <div class="step-number">3</div>
              <h4>Gana Recompensas</h4>
              <p>Recibe pagos por cada contratación exitosa</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<style scoped>
.about {
  padding-top: 64px;
}

.hero {
  background: linear-gradient(135deg, #f9f0ff 0%, #e4f2ff 100%);
  padding: 6rem 2rem;
  text-align: center;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 2rem;
}

.hero h1 {
  font-size: 3.5rem;
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.subtitle {
  font-size: 1.5rem;
  color: #4b5563;
  max-width: 600px;
  margin: 0 auto;
}

.features {
  padding: 6rem 0;
}

.feature-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
  margin-bottom: 4rem;
}

.feature-card {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  text-align: center;
  transition: transform 0.2s;
}

.feature-card:hover {
  transform: translateY(-5px);
}

.icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.feature-card h3 {
  color: #1a1a1a;
  font-size: 1.5rem;
  margin-bottom: 1rem;
}

.feature-card p {
  color: #4b5563;
}

.how-it-works {
  text-align: center;
  margin-top: 6rem;
}

.how-it-works h2 {
  font-size: 2.5rem;
  color: #1a1a1a;
  margin-bottom: 3rem;
}

.steps {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 2rem;
}

.step {
  position: relative;
  padding: 2rem;
}

.step-number {
  width: 40px;
  height: 40px;
  background: #646cff;
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.25rem;
  font-weight: bold;
  margin: 0 auto 1rem;
}

.step h4 {
  color: #1a1a1a;
  font-size: 1.25rem;
  margin-bottom: 0.5rem;
}

.step p {
  color: #4b5563;
}

@media (max-width: 768px) {
  .hero {
    padding: 4rem 1rem;
  }

  .hero h1 {
    font-size: 2.5rem;
  }

  .subtitle {
    font-size: 1.25rem;
  }

  .features {
    padding: 4rem 1rem;
  }

  .how-it-works h2 {
    font-size: 2rem;
  }
}
</style>