require('dotenv').config();
const express = require('express');
const sql = require('mssql');
const cors = require('cors');
const app = express();

const config = {
    server: process.env.DB_SERVER,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_DATABASE,
    options: {
        trustServerCertificate: true, 
        encrypt: true, 
        port: 1433
    }
};

app.use(cors());  
app.use(express.json());


//Realizando pruebas de conectividad 
app.get('/api/data', async (req, res) => {
    try {
        const pool = await sql.connect(config);
        const result = await pool.request().query('SELECT * FROM Usuarios');
        res.json(result.recordset);
    } catch (err) {
        console.error('Error al conectar a la base de datos:', err);
        res.status(500).send(err.message);
    } finally {
        sql.close(); 
    }
});

const port = process.env.PORT || 3000;
app.listen(port, () => {
    console.log(`Servidor corriendo en http://localhost:${port}`);
});