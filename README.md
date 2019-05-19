# Hippocampus

# Intro 

Through the link, you can check and modify the document.

通过链接，你可以查看，并且修改文档

---

- TDD(Tech Design Document)技术设计文档

https://docs.qq.com/doc/DWXFJWVR2a3NWTVl3

If you are not familiar with the framework architecture, please read this document first.

如果您对于系统架构并不熟悉，请先阅读这份文档。

---

#### Project Introduction


#### Introduction to play

#### Setting Project
- At the first, you need open PorjectRoot/Tools
- Open terminal in this path, and input:
```Shell
mono -s [project root/your scenes root] -o [project root/your scene root] -t [project root/tools path] 
```
- Open project by unity, check [ Editor/Always Start From Scene GameRoot ], make sure its checked.
- Right click in [ Project ] window, click [ AssetBundles/BuildAssetbundles ].
- Good job! Now click play to start play demo game!

#### Collaborative Instructions

1. ** clone remote repository** `git clone https://github.com/kwh3884858/Hippocampus`
2. **Enter the warehouse** `cd Hippocampus` Switch to the repository
3. **View branch** `git branch` If you are sure that you are on the master, there is no problem.
4. **Update the repository** `git pull`
5. **Branch description**, `master` is the **main branch**, `dev` is the **development branch**.
6. **Switch to development branch** `git checkout dev`
7. **Modify bugs, ** **Add new features**
   - `git add <filename>` filename can also use * to indicate all
   - `git commit -m "message"` message format below
   - `git push origin dev` push your own changes
   - `git pull` fetches new commits from the repository (if the push fails, the update first resolves the conflict)
8. **Daily development** `git pull` and then connect 6, continue to iterative development

> The master branch is merged by the administrator to prevent bugs from crashing in later projects. There is no need to roll back directly, and the master is a stable version. Every time dev updates new features and there are no bugs, the administrator merges once.

#### format requirement 

**commit** information inside: <type> : <body> <footer>

Type must have, body and footer optional.

Type:

- feat: new features
- fix: fix bug
- docs: documentation
- style: format (code function logic is unchanged)
- refactor: refactoring
- test: test file
- chore: changes to the aids

The common information is as follows:

`git commit -m "fix: modify a certain bug" `

`git commit -m "feat: add new features"

The footer can be omitted later, if the description is clear enough.

If you mistyped the information yourself

`git commit --amend`

Enter the modified commit message to save

`git push <remote> <branch> -f `
   
---
#### 项目介绍


#### 玩法介绍


#### 协作指令

1. **克隆远程仓库** `git clone https://github.com/kwh3884858/Hippocampus`
2. **进入仓库** `cd Hippocampus` 切换到仓库里
3. **查看分支** `git branch`  如果确定自己是在master就没问题。
4. **更新仓库** `git pull` 
5. **分支说明**， `master`是**主分支** ,`dev` 是**开发分支**。
6. **切换到开发分支** `git checkout dev` 
7. **修改bug，** **增加新功能**  
   - `git add <filename>`  filename 也可以用*表示全部
   - `git commit -m "message"` message格式照下面
   - `git push origin dev` 推送自己的改动
   - `git pull` 抓取仓库的新提交 （如果推送失败先更新解决冲突）
8. **日常开发** `git pull` 然后接6，不断迭代开发

> master分支由管理员来合并，防止后期项目出现bug崩溃，不用回滚直接拉，并且master作为稳定版本，每一次dev更新新功能并且没有bug了，管理员合并一次。

#### 格式要求 

**commit** 里面的信息 ：<type> : <body> <footer>

type必须有，body跟footer选填。

type：

- feat：新功能
- fix：修复bug
- docs：文档
- style：格式（代码功能逻辑不变）
- refactor：重构
- test：测试文件
- chore：辅助工具的变动

常见的信息如下：

`git commit -m "fix: 修改某某bug" `

`git commit -m "feat: 增加新功能"` 

后面footer可以省略，描述清楚就行了。

如果自己输错了信息

`git commit --amend` 

输入修改后的commit message 保存

`git push <remote> <branch> -f ` 

