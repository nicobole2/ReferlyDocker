# Fase de construcción
FROM node:18 AS build
WORKDIR /app

# Instalar dependencias
COPY package*.json ./
RUN npm install

# Copiar código fuente y construir
COPY . .
RUN npm run build

# Fase de producción
FROM nginx:alpine
# Copiar la configuración de nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Copiar los archivos construidos
COPY --from=build /app/dist /usr/share/nginx/html

# Exponer puerto 80 para producción
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
