# Auto-update AppRouter.tsx with all page files not yet imported
$routerPath = "C:\Users\A C E R\Downloads\ultimate_erp_system\client\src\hooks\AppRouter.tsx"
$pagesDir = "C:\Users\A C E R\Downloads\ultimate_erp_system\client\src\pages"

# Get all tsx page files
$allPageFiles = Get-ChildItem $pagesDir -Recurse -Filter "*.tsx" | 
    Where-Object { $_.BaseName -ne "index" } |
    Sort-Object FullName

$routerContent = Get-Content $routerPath -Raw
$routerLines = Get-Content $routerPath

# Find last import line index
$lastImportIdx = -1
for ($i = $routerLines.Count - 1; $i -ge 0; $i--) {
    if ($routerLines[$i] -match "^import .* from '") {
        $lastImportIdx = $i
        break
    }
}

# Find closing Route tag (before </Routes>)
$closingRouteIdx = -1
for ($i = $routerLines.Count - 1; $i -ge 0; $i--) {
    if ($routerLines[$i] -match "^\s*</Route>") {
        $closingRouteIdx = $i
        break
    }
}

Write-Host "Last import at line: $($lastImportIdx + 1)"
Write-Host "Closing </Route> at line: $($closingRouteIdx + 1)"

# Collect new imports and routes
$newImports = @()
$newRoutes = @()

foreach ($file in $allPageFiles) {
    $compName = $file.BaseName
    $relPath = $file.FullName.Replace("$pagesDir\", "").Replace("\", "/").Replace(".tsx", "")
    $importLine = "import $compName from '../pages/$relPath';"
    
    # Check if already imported
    if ($routerContent -notmatch [regex]::Escape($compName)) {
        $newImports += $importLine
        
        # Generate route path from component name
        # Convert PascalCase to kebab-case and infer module
        $parts = $relPath -split "/"
        $module = $parts[0].ToLower()
        $pageName = $parts[1] -replace "Page$", "" -replace "ListPage$", "" -replace "FormPage$", "" -replace "ReportPage$", ""
        
        # Convert PascalCase to kebab-case
        $kebab = [regex]::Replace($pageName, '(?<=[a-z])(?=[A-Z])', '-').ToLower()
        
        $routeLine = "          <Route path=`"$module/$kebab`" element={<$compName />} />"
        $newRoutes += $routeLine
    }
}

Write-Host "New imports to add: $($newImports.Count)"
Write-Host "New routes to add: $($newRoutes.Count)"

if ($newImports.Count -eq 0) {
    Write-Host "No new pages to add!"
    exit 0
}

# Build new content
$lines = [System.Collections.Generic.List[string]]($routerLines)

# Insert imports after last import (insert in reverse to maintain correct positions)
$importBlock = $newImports -join "`n"
$lines.Insert($lastImportIdx + 1, "`n// --- Round 2 New Pages ---`n$importBlock")

# Re-find closing route index (it shifted due to import insertion)
$newLines = $lines.ToArray()
$closingRouteIdx2 = -1
for ($i = $newLines.Count - 1; $i -ge 0; $i--) {
    if ($newLines[$i] -match "^\s*</Route>") {
        $closingRouteIdx2 = $i
        break
    }
}

$routeBlock = $newRoutes -join "`n"
$lines2 = [System.Collections.Generic.List[string]]($newLines)
$lines2.Insert($closingRouteIdx2, "`n          {/* Round 2 Pages */}`n$routeBlock")

$finalContent = $lines2 -join "`n"
$finalContent | Set-Content $routerPath -Encoding UTF8 -NoNewline

$finalLineCount = (Get-Content $routerPath | Measure-Object -Line).Lines
Write-Host "AppRouter.tsx updated: $finalLineCount lines"

# Deduplicate imports
Write-Host "Deduplicating..."
$content = Get-Content $routerPath
$seen = @{}
$deduped = @()
foreach ($line in $content) {
    $trimmed = $line.TrimEnd()
    if ($trimmed -match "^import .* from '") {
        if (-not $seen.ContainsKey($trimmed)) {
            $seen[$trimmed] = $true
            $deduped += $line
        }
    } elseif ($trimmed -match '<Route path="') {
        if ($trimmed -match '<Route path="([^"]+)"') {
            $routePath = $matches[1]
            if (-not $seen.ContainsKey("ROUTE:$routePath")) {
                $seen["ROUTE:$routePath"] = $true
                $deduped += $line
            }
        } else {
            $deduped += $line
        }
    } else {
        $deduped += $line
    }
}
$deduped -join "`n" | Set-Content $routerPath -Encoding UTF8 -NoNewline

$finalLineCount2 = (Get-Content $routerPath | Measure-Object -Line).Lines
Write-Host "After deduplication: $finalLineCount2 lines"

$finalImports = (Get-Content $routerPath | Where-Object { $_ -match "^import .* from '" }).Count
$finalRoutes = (Get-Content $routerPath | Where-Object { $_ -match '<Route path="' }).Count
Write-Host "Total imports: $finalImports | Total routes: $finalRoutes"
