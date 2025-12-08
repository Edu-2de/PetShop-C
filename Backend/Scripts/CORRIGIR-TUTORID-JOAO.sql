-- Script para corrigir o TutorId do usuário João Santos
-- Problema: João Santos tem pets cadastrados mas seu usuário não tem TutorId vinculado

-- Primeiro, vamos identificar o problema
SELECT 
    u.UsuarioId,
    u.Nome as NomeUsuario,
    u.Email,
    u.TutorId as TutorIdNoUsuario,
    t.TutorId as TutorIdReal,
    t.Nome as NomeTutor,
    (SELECT COUNT(*) FROM Animais WHERE TutorId = t.TutorId) as QtdPets
FROM Usuarios u
LEFT JOIN Tutores t ON t.Email = u.Email
WHERE u.Email = 'joao.santos@email.com';

-- Atualizar o TutorId do usuário João Santos com base no tutor existente
UPDATE Usuarios
SET TutorId = (
    SELECT TOP 1 TutorId 
    FROM Tutores 
    WHERE Email = 'joao.santos@email.com'
)
WHERE Email = 'joao.santos@email.com' 
  AND TutorId IS NULL;

-- Verificar a correção
SELECT 
    u.UsuarioId,
    u.Nome,
    u.Email,
    u.TutorId,
    t.Nome as NomeTutor,
    (SELECT COUNT(*) FROM Animais WHERE TutorId = u.TutorId) as QtdPets
FROM Usuarios u
LEFT JOIN Tutores t ON t.TutorId = u.TutorId
WHERE u.Email = 'joao.santos@email.com';

-- Atualizar também outros usuários que possam ter o mesmo problema
UPDATE Usuarios
SET TutorId = (
    SELECT TOP 1 t.TutorId 
    FROM Tutores t 
    WHERE t.Email = Usuarios.Email
)
WHERE TutorId IS NULL
  AND EXISTS (
      SELECT 1 FROM Tutores t WHERE t.Email = Usuarios.Email
  );

-- Verificar todos os usuários agora
SELECT 
    u.UsuarioId,
    u.Nome,
    u.Email,
    u.Cargo,
    u.TutorId,
    u.FuncionarioId,
    CASE 
        WHEN u.TutorId IS NOT NULL THEN 'Tutor Vinculado'
        WHEN u.FuncionarioId IS NOT NULL THEN 'Funcionário Vinculado'
        WHEN u.Cargo = 'Admin' THEN 'Admin (sem vínculo)'
        ELSE '⚠️ SEM VÍNCULO'
    END as Status
FROM Usuarios u
ORDER BY u.UsuarioId;
