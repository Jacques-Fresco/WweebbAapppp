const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://129.168.0.102:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:20803';

const urlApiServer = 'https://192.168.0.102:7089';

const context =  [
    //"/weatherforecast",
    "/api",
    
    //"/chatHub"
];

const contextChat = [
    //"/chatmessages"
    "/message"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: urlApiServer,
    secure: false,
    headers: {
      Connection: "Keep-Alive"
    }
  });

    const chatHubProxy = createProxyMiddleware(contextChat, {
      target: urlApiServer,
      ws: true,
      changeOrigin: true,
      secure: false,
      /*headers: {
          Connection: 'Keep-Alive'
      },
      wsOptions: {
          rejectUnauthorized: false
      }*/
  });

  app.use(chatHubProxy);
  app.use(appProxy);
};
